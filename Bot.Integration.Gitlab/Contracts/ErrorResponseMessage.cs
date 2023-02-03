using System.Text.Json.Serialization;

namespace Bot.Integration.Gitlab.Contracts;

public class ErrorResponseMessage
{
    [JsonPropertyName("message")]
    public string Message { get; init; } = string.Empty;
}
