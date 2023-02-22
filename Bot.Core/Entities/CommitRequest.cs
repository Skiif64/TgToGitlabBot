namespace Bot.Core.Entities;

public class CommitRequest : IDisposable
{
    public string From { get; init; } = string.Empty;
    public long FromChatId { get; init; }
    public string FileName { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public Stream? Content { get; init; }
    public string? ContentType { get; init; }

    public void Dispose()
    {
        if(Content is not null)
            Content.Dispose();
    }
}
