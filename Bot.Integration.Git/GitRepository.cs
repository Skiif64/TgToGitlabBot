using Bot.Core.Abstractions;
using Bot.Core.Entities;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Test.CMD")]
namespace Bot.Integration.Git;

internal class GitRepository : IGitlabService
{
    private readonly GitOptions _options;
    private readonly Identity _identity;
    private readonly ILogger<GitRepository>? _logger;
    private CredentialsHandler _credentialsHandler;
    private readonly object _lock = new object();
    public GitRepository(GitOptions options, ILogger<GitRepository>? logger = null)
    {
        _options = options;
        _identity = new Identity(_options.Username, _options.Email);
        _logger = logger;

        _credentialsHandler = (url, user, type) => new UsernamePasswordCredentials
        {
            Username = _options.Username,
            Password = _options.AccessToken
        };
        Initialize();
    }

    public async Task<bool> CommitFileAndPushAsync(CommitInfo file, CancellationToken cancellationToken = default)
    {
        return await Task
            .Factory
            .StartNew(() => CommitFileAndPush(file), cancellationToken);
    }

    public bool CommitFileAndPush(CommitInfo info)
    {
        lock (_lock)
        {
            try
            {

                var signature = new Signature(_identity, DateTimeOffset.UtcNow);
                if (info.Content is null)
                    throw new ArgumentNullException(nameof(info.Content));
                string filepath;
                if (_options.FilePath is not null)
                    filepath = Path.Combine(_options.FilePath, info.FileName);
                else
                    filepath = info.FileName;
                using var repository = new Repository(_options.LocalPath);
                using (var fileStream = new FileStream(Path.Combine(_options.LocalPath, filepath), FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using var writer = new BinaryWriter(fileStream);
                    using var reader = new BinaryReader(info.Content);
                    writer.Write(reader.ReadBytes((int)info.Content.Length));
                }
                Commands.Stage(repository, filepath);
                var commit = repository.Commit(info.Message, signature, signature);
                Push();
            }
            catch (LibGit2SharpException exception)
            {
                _logger?.LogError($"Exception occured while commiting file: {exception.Message}");
                return false;
            }
            return true;
        }
    }

    public void Push()
    {
        using var repository = new Repository(_options.LocalPath);
        var remote = repository.Network.Remotes["origin"];

        repository.Network.Push(remote, $@"refs/heads/{_options.Branch}", new PushOptions
        {
            CredentialsProvider = _credentialsHandler
        });
    }

    public void Initialize()
    {
        if (Repository.IsValid(_options.LocalPath))
            return;

        Repository.Clone(_options.Url, _options.LocalPath, new CloneOptions
        {
            BranchName = _options.Branch,
            CredentialsProvider = _credentialsHandler
        });
    }
}
