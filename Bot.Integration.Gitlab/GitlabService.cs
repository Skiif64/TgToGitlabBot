using Bot.Core.Abstractions;
using Bot.Core.Entities;
using Bot.Integration.Gitlab.Abstractions;
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

    public async Task<bool> CommitFileAsync(CommitInfo file, CancellationToken cancellationToken = default)
    {     
        var result = await _client.SendAsync(
             new CreateRequest(file.Message,
             new[]
             {
                 new CreateAction(_options.FilePath + file.FileName, file.Content)
             },
             _options),
             cancellationToken
             );

        if (result is not null)
            _logger.LogInformation($"Commited {file.FileName} from {file.From} to branch {_options.BranchName}");
        else
            _logger.LogCritical($"Error during commiting file: "); //TODO: print exception

        return true;
    }
}
