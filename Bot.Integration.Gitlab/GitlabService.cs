using Bot.Core.Abstractions;
using Bot.Core.Entities;
using Bot.Core.Options;
using Bot.Integration.Gitlab.Abstractions;
using Bot.Integration.Gitlab.Entities;
using Bot.Integration.Gitlab.Primitives;
using Bot.Integration.Gitlab.Requests;
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

    public async Task CommitFileAsync(CommitInfo file, CancellationToken cancellationToken = default)
    { //TODO: add returning        
        var result = await _client.SendAsync(
             new CreateRequest(file.Message,
             new[]
             {
                 new CreateAction(file.FileName, file.Content)
             },
             _options),
             cancellationToken
             );

        if (result)
            _logger.LogInformation($"Commited {file.FileName} from {file.From} to branch {_options.BranchName}"); //TODO: more infomational logging
        else
            _logger.LogCritical("Error during commiting file");
    }
}
