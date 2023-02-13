﻿using Bot.Core.Abstractions;
using Bot.Core.Entities;
using Bot.Core.Exceptions;
using Bot.Core.ResultObject;
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

    public async Task<Result<bool>> CommitFileAndPushAsync(CommitInfo file, CancellationToken cancellationToken = default)
    {
        return await Task
            .Factory
            .StartNew(() => CommitFileAndPush(file), cancellationToken);
    }

    public Result<bool> CommitFileAndPush(CommitInfo info)
    {
        if (!_options.ChatOptions.TryGetValue(info.FromChatId.ToString(), out var optionsSection))
        {
            _logger?.LogWarning($"Configuration for chat {info.FromChatId} is not set!");
            return new ErrorResult<bool>(new ConfigurationNotSetException(info.FromChatId));
        }        

        try
        {
            if (!_initialized)
            {
                _credentialsHandler = (url, user, type) => new UsernamePasswordCredentials
                {
                    Username = _options.Username,
                    Password = optionsSection.AccessToken
                };
                _initialized = new InitializeCommand(optionsSection, _credentialsHandler)
                    .Execute(null!);
            }
            using var repository = new Repository(optionsSection.LocalPath);

            var signature = new Signature(_identity, DateTimeOffset.UtcNow);           
            string filepath;
            if (optionsSection.FilePath is not null)
                filepath = Path.Combine(optionsSection.FilePath, info.FileName);
            else
                filepath = info.FileName;
            new PullChangesCommand(signature, _credentialsHandler)
                .Execute(repository);
            new AddFileCommand(info, optionsSection)
                .Execute(repository);
            new StageAndCommitCommand(filepath, info.Message, signature)
                .Execute(repository);
            new PushCommand(optionsSection, _credentialsHandler)
                .Execute(repository);
        }
        catch (LibGit2SharpException exception)
        {            
            _logger?.LogError($"Exception occured while commiting file: {exception}");
            return new ErrorResult<bool>(HandleLibGitException(exception));
        }
        _logger?.LogInformation($"Succesufully commited and push file {info.FileName}to project {optionsSection.Url}, branch {optionsSection.Branch}");
        return new SuccessResult<bool>(true);
    }
    
    private string CacheFile(string filepath)
    {
        var cachedFilepath = filepath + ".cached";

        using var sourceFileStream = File.OpenRead(filepath);
        using var sourceBinaryReader = new BinaryReader(sourceFileStream);
        using var destFileStream = File.Create(cachedFilepath);
        using var destBinaryWriterWriter = new BinaryWriter(destFileStream);

        destBinaryWriterWriter.Write(sourceBinaryReader.ReadBytes((int)sourceFileStream.Length));
        return cachedFilepath;
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
