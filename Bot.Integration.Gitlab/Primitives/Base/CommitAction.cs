using System.Text.Json.Serialization;

namespace Bot.Integration.Gitlab.Primitives.Base;

internal abstract class CommitAction
{
    [JsonPropertyName("action")]        
    public abstract ActionType Action { get; }
    [JsonPropertyName("file_path")]
    public string FilePath { get; init; } = string.Empty;
    [JsonPropertyName("content")]
    public string? Content { get; init; }


}
