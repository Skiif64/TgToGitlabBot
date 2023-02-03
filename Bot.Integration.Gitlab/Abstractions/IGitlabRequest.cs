using System.Text.Json.Serialization;

namespace Bot.Integration.Gitlab.Abstractions
{
    public interface IGitlabRequest
    {
        [JsonIgnore]
        HttpMethod Method { get; }
        [JsonIgnore]
        string Url { get; }
        [JsonIgnore]
        string? AccessToken { get; }

        HttpContent? ToHttpContent();
    }
}