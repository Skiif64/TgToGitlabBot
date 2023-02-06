using System.Text.Json.Serialization;

namespace Bot.Integration.Gitlab.Responses;

internal class GetFileResponse
{
    [JsonPropertyName("file_name")]
    public string FileName { get; set; } = string.Empty;
    [JsonPropertyName("file_path")]
    public string FilePath { get; set; } = string.Empty;
    [JsonPropertyName("size")]
    public long Size { get; set; }
    [JsonPropertyName("encoding")]
    public string Encoding { get; set; } = string.Empty;
    [JsonPropertyName("content_sha256")]
    public string ContentSha256 { get; set; } = string.Empty;
    [JsonPropertyName("ref")]
    public string Ref { get; set; } = string.Empty;
    [JsonPropertyName("blob_id")]
    public string BlobId { get; set; } = string.Empty;
    [JsonPropertyName("commit_id")]
    public string CommitId { get; set; } = string.Empty;
    [JsonPropertyName("last_commit_id")]
    public string LastCommitId { get; set; } = string.Empty;
    [JsonPropertyName("execute_filemode")]
    public bool ExecuteFilemode { get; set; }
}
