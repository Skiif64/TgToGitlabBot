using Bot.Core.Abstractions;
using Bot.Core.Entities;
using Bot.Core.Options;
using Bot.Integration.Gitlab.Abstractions;
using Bot.Integration.Gitlab.Requests;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Bot.Integration.Gitlab;

public class GitlabService : IGitlabService
{    
    private readonly GitLabOptions _options;
    private readonly IGitlabClient _client;

    public GitlabService(IGitlabClient client, GitLabOptions options)
    {
        _client = client;
        _options = options;
    }

    public GitlabService(IGitlabClient client, IOptions<GitLabOptions> options) : this(client, options.Value)
    {

    }

    public async Task CommitFileAsync(CommitInfo file, CancellationToken cancellationToken = default)
    {
          await _client.SendAsync(
              new CommitRequest(file.MapToRequest(_options), _options),
              cancellationToken
              );
    }    
}
