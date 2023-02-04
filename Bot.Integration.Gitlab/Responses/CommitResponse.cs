using System.Text.Json.Serialization;

namespace Bot.Integration.Gitlab.Responses;

public class CommitResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("short_id")]
    public string ShortId { get; set; }
    [JsonPropertyName("title")]
    public string Title { get; set; }
    [JsonPropertyName("author_name")]
    public string AuthorName { get; set; }
    [JsonPropertyName("author_email")]
    public string AuthorEmail { get; set; }
    [JsonPropertyName("committer_name")]
    public string CommitterName { get; set; }
    [JsonPropertyName("committer_email")]
    public string CommitterEmail { get; set; }
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
    [JsonPropertyName("message")]
    public string Message { get; set; }
    [JsonPropertyName("parent_ids")]
    public string[] ParentIds { get; set; }
    [JsonPropertyName("committed_date")]
    public DateTime CommittedDate { get; set; }
    [JsonPropertyName("authored_date")]
    public DateTime AuthoredDate { get; set; }
    [JsonPropertyName("stats")]
    public Stats Stats { get; set; }
    [JsonPropertyName("status")]
    public string? Status { get; set; }
    [JsonPropertyName("web_url")]
    public string WebUrl { get; set; }
}

public class Stats
{
    [JsonPropertyName("additions")]
    public int Additions { get; set; }
    [JsonPropertyName("deletions")]
    public int Deletions { get; set; }
    [JsonPropertyName("total")]
    public int Total { get; set; }
}
