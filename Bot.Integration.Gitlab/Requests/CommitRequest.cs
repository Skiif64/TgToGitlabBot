using Bot.Core.Options;
using Bot.Integration.Gitlab.Abstractions;
using Bot.Integration.Gitlab.Entities;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

[assembly: InternalsVisibleTo("Test.CMD")]

namespace Bot.Integration.Gitlab.Requests;

internal class CommitRequest : IGitlabRequest
{
    public HttpMethod Method { get; } = HttpMethod.Post;
    public string Url { get; } = "/api/v4/projects/{0}/repository/commits";     
    public string? AccessToken { get; }

    [JsonPropertyName("branch")]
    public string Branch { get; set; } = string.Empty;
    [JsonPropertyName("commit_message")]
    public string CommitMessage { get; set; } = string.Empty;
    [JsonPropertyName("author_name")]
    public string? AuthorName { get; set; }
    [JsonPropertyName("author_email")]
    public string? AuthorEmail { get; set; }
    [JsonPropertyName("actions")]
    public CommitActionDto[] Actions { get; set; } = Array.Empty<CommitActionDto>();

    public CommitRequest(GitLabOptions options)
    {
        Url = string.Format(Url, options.ProjectNamespace+"%2F"+options.ProjectName);
        AccessToken = options.AccessToken;        
    }

    public HttpContent? ToHttpContent() => JsonContent.Create(this);
}
