using System.Net.Http.Json;

namespace Bot.Integration.Gitlab.Requests.Base;

internal abstract class RequestBase<TResponse> : IGitlabRequest<TResponse>
{
    public abstract HttpMethod Method { get; }
    public abstract string Url { get; }
    public string? AccessToken { get; }

    public RequestBase(GitLabOptions options)
    {        
        AccessToken = options.AccessToken;
    }

    public abstract HttpContent? ToHttpContent();

}
