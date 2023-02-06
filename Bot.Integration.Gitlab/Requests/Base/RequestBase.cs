using System.Net.Http.Json;

namespace Bot.Integration.Gitlab.Requests.Base;

internal abstract class RequestBase<TResponse> : IGitlabRequest<TResponse>
{
    public abstract HttpMethod Method { get; }
    public abstract string Url { get; }
    public string? AccessToken { get; }
    public Dictionary<string, string?> Headers { get; } = new();

    public RequestBase(GitlabChatOptions options)
    {        
        AccessToken = options.AccessToken;
        if (AccessToken is not null)
            Headers.Add("PRIVATE-TOKEN", AccessToken);
    }

    public abstract HttpContent? ToHttpContent();

}
