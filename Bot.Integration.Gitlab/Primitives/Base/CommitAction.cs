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
    [JsonPropertyName("encoding")]
    public string? Encoding { get; init; } = "base64";

    protected CommitAction(string filepath, string? content)
    {
        if (string.IsNullOrWhiteSpace(filepath))
            throw new ArgumentNullException(nameof(filepath));
        FilePath = filepath;
        Content = content;
    }

    protected CommitAction(string filepath, Stream contentStream)
    {
        if (contentStream is null)
            throw new ArgumentNullException(nameof(contentStream));
        FilePath = filepath;
        using (var br = new BinaryReader(contentStream))
        {
            contentStream.Position = 0;
            Content = Convert.ToBase64String(br.ReadBytes((int)br.BaseStream.Length));
        }
    }


}
