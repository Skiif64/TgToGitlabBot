using Bot.Core.Abstractions;
using Bot.Core.Entities;
using Bot.Core.Exceptions;
using Bot.Core.ResultObject;
using Bot.Integration.Git.GitCommands;
using Bot.Integration.Git.GitCommands.AddFile;
using Bot.Integration.Git.GitCommands.CacheFile;
using Bot.Integration.Git.GitCommands.Initialize;
using Bot.Integration.Git.GitCommands.PullChanges;
using Bot.Integration.Git.GitCommands.Push;
using Bot.Integration.Git.GitCommands.Rollback;
using Bot.Integration.Git.GitCommands.StageAndCommit;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Bot.Integration.Git.Tests")]
namespace Bot.Integration.Git;

internal class GitRepository : IGitlabService
{
    private readonly GitOptions _options;
    private readonly ILogger<GitRepository>? _logger;
    private readonly ISender _sender;
    private CredentialsHandler _credentialsHandler = null!;
    private Identity _identity;
    private bool _initialized;
    public GitRepository(ISender sender, GitOptions options, ILogger<GitRepository>? logger = null)
    {
        _sender = sender;
        _options = options;
        _logger = logger;
        _identity = new Identity(_options.Username, _options.Email);

    }

    public GitRepository(ISender sender, IOptionsSnapshot<GitOptions> options, ILogger<GitRepository>? logger = null)
        : this(sender, options.Value, logger)
    {

    }

    public async Task<Result<bool>> CommitFileAndPushAsync(CommitInfo info, CancellationToken cancellationToken = default)
    {
        if (!_options.ChatOptions.TryGetValue(info.FromChatId.ToString(), out var optionsSection))
        {
            _logger?.LogWarning($"Configuration for chat {info.FromChatId} is not set!");
            return new ErrorResult<bool>(new ConfigurationNotSetException(info.FromChatId));
        }

        string filepath = string.Empty;
        string repositoryFilepath = string.Empty;
        string? cachedFilepath = null;

        if (!_initialized)
        {
            _credentialsHandler = (url, user, type) => new UsernamePasswordCredentials
            {
                Username = _options.Username,
                Password = optionsSection.AccessToken
            };
            await _sender.Send(new InitializeCommand(optionsSection, _credentialsHandler),
                cancellationToken);
            _initialized = true;
        }
        using var repository = new Repository(optionsSection.LocalPath);
        try
        {
            var signature = new Signature(_identity, DateTimeOffset.UtcNow);

            filepath = optionsSection.FilePath is not null
                ? filepath = Path.Combine(optionsSection.FilePath, info.FileName)
                : filepath = info.FileName;
            repositoryFilepath = Path.Combine(optionsSection.LocalPath, filepath);
            if (File.Exists(repositoryFilepath))
                cachedFilepath = await _sender.Send(new CacheFileCommand(repositoryFilepath),
                    cancellationToken);

            await _sender.Send(new PullChangesCommand(repository, signature, _credentialsHandler),
                cancellationToken);
            await _sender.Send(new AddFileCommand(info.Content!, repositoryFilepath),
                cancellationToken);
            await _sender.Send(new StageAndCommitCommand(repository, filepath, info.Message, signature),
                cancellationToken);
            await _sender.Send(new PushCommand(repository, _credentialsHandler, optionsSection.Branch),
                cancellationToken);

            if (cachedFilepath != null)
                File.Delete(cachedFilepath);
        }
        catch (LibGit2SharpException exception)
        {
            if (repository is not null)
                await _sender.Send(new RollbackCommand(repository,
                    repositoryFilepath, cachedFilepath,
                    exception is EmptyCommitException),
                    cancellationToken);

            _logger?.LogError($"Exception occured while commiting file: {exception}");
            return new ErrorResult<bool>(HandleLibGitException(exception));
        }
        _logger?.LogInformation($"Succesufully commited and push file {info.FileName}to project {optionsSection.Url}, branch {optionsSection.Branch}");
        return new SuccessResult<bool>(true);
    }


    private Exception HandleLibGitException(LibGit2SharpException exception)
    {
        return exception switch
        {
            EmptyCommitException => new GitException("Пустой коммит, возможно в файле отсутствуют какие-либо изменения"),
            _ => new GitException("Ошибка Git")
        };
    }
}
