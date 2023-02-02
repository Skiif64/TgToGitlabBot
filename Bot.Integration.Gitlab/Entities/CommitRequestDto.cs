using System.Text.Json.Serialization;

namespace Bot.Integration.Gitlab.Entities;

internal class CommitRequestDto
{    
    [JsonPropertyName("branch")]
    public string Branch { get; init; } = string.Empty;
    [JsonPropertyName("commit_message")]
    public string CommitMessage { get; init; } = string.Empty;
    [JsonPropertyName("actions")]
    public CommitActionDto[] Actions { get; init; } = Array.Empty<CommitActionDto>();
}
