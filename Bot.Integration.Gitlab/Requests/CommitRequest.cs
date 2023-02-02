using Bot.Core.Options;
using Bot.Integration.Gitlab.Abstractions;
using Bot.Integration.Gitlab.Entities;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
[assembly: InternalsVisibleTo("Test.CMD")]

namespace Bot.Integration.Gitlab.Requests;

internal class CommitRequest : IGitlabRequest
{
    public HttpMethod Method { get; } = HttpMethod.Post;
    public string Url { get; } = "/projects/{0}/repository/commits"; 
    public HttpContent Content { get; }
    public string? AccessToken { get; }

    public CommitRequest(CommitRequestDto message, GitLabOptions options)
    {
        Url = string.Format(Url, options.ProjectPath);
        AccessToken = options.AccessToken;
        var json = JsonSerializer.Serialize(message);
        Content = new StringContent(json, Encoding.UTF8, "application/json");
    }    
}
