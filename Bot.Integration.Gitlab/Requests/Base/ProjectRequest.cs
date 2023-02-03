using Bot.Core.Options;
using System.Net.Http.Json;

namespace Bot.Integration.Gitlab.Requests.Base;

internal abstract class ProjectRequest : IGitlabRequest
{
    public abstract HttpMethod Method { get; }
    public virtual string Url { get; }
    public string? AccessToken { get; }

    public ProjectRequest(GitLabOptions options)
    {
        Url = string.Format(Url, options.ProjectNamespace + "%2F" + options.ProjectName);
        AccessToken = options.AccessToken;
    }

    public virtual HttpContent? ToHttpContent() => JsonContent.Create(this);

}
