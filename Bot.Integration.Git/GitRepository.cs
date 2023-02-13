using Bot.Core.Abstractions;
using Bot.Core.Entities;
using Bot.Integration.Git.GitCommands;
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
    private CredentialsHandler _credentialsHandler = null!;
    private Identity _identity;
    private bool _initialized;
    public GitRepository(GitOptions options, ILogger<GitRepository>? logger = null)
    {
        _options = options;
        _logger = logger;
        _identity = new Identity(_options.Username, _options.Email);

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
        if (!_options.ChatOptions.TryGetValue(info.FromChatId.ToString(), out var optionsSection))
        {
            _logger?.LogWarning($"Configuration for chat {info.FromChatId} is not set!");
            return false;
        }        

        try
        {
            using var repository = new Repository(optionsSection.LocalPath);
            if (!_initialized)
                _initialized = new InitializeCommand(optionsSection, _credentialsHandler)
                    .Execute(repository);

            var signature = new Signature(_identity, DateTimeOffset.UtcNow);           
            string filepath;
            if (optionsSection.FilePath is not null)
                filepath = Path.Combine(optionsSection.FilePath, info.FileName);
            else
                filepath = info.FileName;

            new AddFileCommand(info, optionsSection, _identity, signature)
                .Execute(repository);
            new StageAndCommitCommand(filepath, info.Message, signature)
                .Execute(repository);
            new PushCommand(optionsSection, _credentialsHandler)
                .Execute(repository);
        }
        catch (LibGit2SharpException exception)
        {
            Console.WriteLine(exception);
            _logger?.LogError($"Exception occured while commiting file: {exception.Message} {exception}");
            return false;
        }
        return true;
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

    
}
