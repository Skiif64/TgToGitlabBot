using System.Text.Json.Serialization;

namespace Bot.Integration.Gitlab.Requests.Base;

public interface IGitlabRequest<TResponse>
{
    [JsonIgnore]
    HttpMethod Method { get; }
    [JsonIgnore]
    string Url { get; }
    [JsonIgnore]
    string? AccessToken { get; }
    [JsonIgnore]
    Dictionary<string, string?> Headers { get; }

    HttpContent? ToHttpContent();
}
