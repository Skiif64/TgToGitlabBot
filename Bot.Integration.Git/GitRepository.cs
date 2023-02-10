using Bot.Core.Abstractions;
using Bot.Core.Entities;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Test.CMD")]
namespace Bot.Integration.Git;

internal class GitRepository : IGitlabService
{
    private readonly GitOptions _options;    
    private readonly ILogger<GitRepository>? _logger;
    private readonly object _lock = new object();
    private CredentialsHandler _credentialsHandler;
    private Identity _identity;
    private bool _initialized;
    public GitRepository(GitOptions options, ILogger<GitRepository>? logger = null)
    {
        _options = options;                
        _logger = logger;              
    }

    public GitRepository(IOptionsSnapshot<GitOptions> options, ILogger<GitRepository>? logger = null)
        : this(options.Value, logger)
    {

    }

    public async Task<bool> CommitFileAndPushAsync(CommitInfo file, CancellationToken cancellationToken = default)
    {
        return await Task
            .Factory
            .StartNew(() => CommitFileAndPush(file), cancellationToken);
    }

    public bool CommitFileAndPush(CommitInfo info)
    {
        if(!_options.ChatOptions.TryGetValue(info.FromChatId.ToString(), out var optionsSection))
        {
            _logger?.LogWarning($"Configuration for chat {info.FromChatId} is not set!");
            return false;
        }
        if (!_initialized)
            Initialize(optionsSection);
        lock (_lock)
        {
            try
            {

                var signature = new Signature(_identity, DateTimeOffset.UtcNow);
                if (info.Content is null)
                    throw new ArgumentNullException(nameof(info.Content));
                string filepath;
                if (optionsSection.FilePath is not null)
                    filepath = Path.Combine(optionsSection.FilePath, info.FileName);
                else
                    filepath = info.FileName;
                using var repository = new Repository(optionsSection.LocalPath);
                using (var fileStream = new FileStream(Path.Combine(optionsSection.LocalPath, filepath), FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using var writer = new BinaryWriter(fileStream);
                    using var reader = new BinaryReader(info.Content);
                    writer.Write(reader.ReadBytes((int)info.Content.Length));
                }
                Commands.Stage(repository, filepath);
                var commit = repository.Commit(info.Message, signature, signature);
                Push(optionsSection);
            }
            catch (LibGit2SharpException exception)
            {
                _logger?.LogError($"Exception occured while commiting file: {exception.Message}");
                return false;
            }
            return true;
        }
    }

    private void Push(GitOptionsSection optionsSection)
    {
        using var repository = new Repository(optionsSection.LocalPath);
        var remote = repository.Network.Remotes["origin"];

        repository.Network.Push(remote, $@"refs/heads/{optionsSection.Branch}", new PushOptions
        {
            CredentialsProvider = _credentialsHandler
        });
    }

    private void Initialize(GitOptionsSection optionsSection)
    {
        if (Repository.IsValid(optionsSection.LocalPath) || _initialized)
            return;
        _identity = new Identity(optionsSection.Username, optionsSection.Email);
        _credentialsHandler = (url, user, type) => new UsernamePasswordCredentials
        {
            Username = optionsSection.Username,
            Password = optionsSection.AccessToken
        };
        if (!Directory.Exists(optionsSection.LocalPath))
            Directory.CreateDirectory(optionsSection.LocalPath);
        Repository.Clone(optionsSection.Url, optionsSection.LocalPath, new CloneOptions
        {
            BranchName = optionsSection.Branch,
            CredentialsProvider = _credentialsHandler
        });
        _initialized = true;
    }
}
