﻿using Bot.Core.Abstractions;
using Bot.Core.Entities;
using Bot.Integration.Gitlab.Abstractions;
using Bot.Integration.Gitlab.Exceptions;
using Bot.Integration.Gitlab.Primitives;
using Bot.Integration.Gitlab.Primitives.Base;
using Bot.Integration.Gitlab.Requests;
using Bot.Integration.Gitlab.Responses;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bot.Integration.Gitlab;

public class GitlabService : IGitlabService
{
    private readonly GitLabOptions _options;
    private readonly IGitlabClient _client;
    private readonly ILogger<IGitlabService> _logger;

    public GitlabService(IGitlabClient client, GitLabOptions options, ILogger<IGitlabService> logger)
    {
        _client = client;
        _options = options;
        _logger = logger;
    }

    public GitlabService(IGitlabClient client, IOptions<GitLabOptions> options, ILogger<GitlabService> logger)
        : this(client, options.Value, logger)
    {

    }

    public async Task<bool> CommitFileAsync(CommitInfo file, CancellationToken cancellationToken = default)
    {
        var chatOptions = _options.ChatOptions[file.FromChatId.ToString()];
        try
        {
            if (await FileExists(file, cancellationToken, chatOptions))
                await UpdateFile(file, cancellationToken, chatOptions);
            else
                await CreateNewFile(file, cancellationToken, chatOptions);

            _logger.LogInformation($"Commited {file.FileName} from {file.From} to branch {chatOptions.BranchName}");
            return true;
        }
        catch (ValidationException exception)
        {
            _logger.LogCritical($"Error during commiting file: {exception.Message}");
        }

        return false;
    }

    private async Task<bool> FileExists(CommitInfo file, CancellationToken cancellationToken, GitlabChatOptions options)
    {
        try
        {
            await _client.SendAsync(
                new GetFileRequest(options.FilePath + file.FileName, options),
                cancellationToken
                );
            return true;
        }
        catch (NotFoundException exception)
        {
            return false;
        }
        catch (ValidationException exception)
        {
            _logger.LogCritical($"Error during checking existing file: {exception.Message}");
        }
        return false;
    }

    private async Task<CommitResponse> UpdateFile(CommitInfo file, CancellationToken cancellationToken, GitlabChatOptions options)
    {
        return await _client.SendAsync(
                        new UpdateRequest(file.Message,
                        new[]
                        {
                    new UpdateAction(options.FilePath + file.FileName, file.Content)
                        },
                        options),
                        cancellationToken);
    }

    private async Task<CommitResponse> CreateNewFile(CommitInfo file, CancellationToken cancellationToken, GitlabChatOptions options)
    {
        return await _client.SendAsync(
                         new CreateRequest(file.Message,
                         new[]
                         {
                 new CreateAction(options.FilePath + file.FileName, file.Content)
                         },
                         options),
                         cancellationToken
                         );
    }
}
