namespace Bot.Core.Entities;

public class CommitInfo
{
    public string From { get; init; } = string.Empty;
    public long FromChatId { get; init; }
    public string FileName { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public string? Content { get; init; }

}
