using System.Text.Json.Serialization;

namespace Bot.Integration.Gitlab.Entities
{
    internal class CommitActionDto
    {
        [JsonPropertyName("action")]
        public string Action { get; init; } = string.Empty;
        [JsonPropertyName("file_path")]
        public string FilePath { get; init; } = string.Empty;
        [JsonPropertyName("content")]
        public string Content { get; init; } = string.Empty;
    }
}