using Bot.Core.Abstractions;
using Bot.Core.Entities;
using Bot.Core.Options;
using Bot.Integration.Gitlab.Abstractions;
using Bot.Integration.Gitlab.Requests;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Bot.Integration.Gitlab;

public class GitlabService : IGitlabService
{    
    private readonly GitLabOptions _options;
    private readonly IGitlabClient _client;
    private readonly ILogger<GitlabService> _logger;

    public GitlabService(IGitlabClient client, GitLabOptions options, ILogger<GitlabService> logger)
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
              new CommitRequest(file.MapToRequest(_options), _options),
              cancellationToken
              );

        if (result)
            _logger.LogInformation($"Commited {file.FileName} from {file.From}");
        else
            _logger.LogCritical("Error during commiting file");        
    }    
}
