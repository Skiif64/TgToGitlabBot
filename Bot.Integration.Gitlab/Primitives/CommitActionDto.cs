using System.Text.Json.Serialization;

namespace Bot.Integration.Gitlab.Primitives;

internal abstract class CommitActionDto
{
    [JsonPropertyName("action")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public abstract ActionEnum Action { get; }
    [JsonPropertyName("file_path")]
    public string FilePath { get; init; } = string.Empty;
    [JsonPropertyName("content")]
    public string? Content { get; init; }

    
}
