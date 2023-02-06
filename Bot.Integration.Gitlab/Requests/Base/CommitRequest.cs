using Bot.Integration.Gitlab.Primitives.Base;
using Bot.Integration.Gitlab.Requests.Base;
using Bot.Integration.Gitlab.Responses;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

[assembly: InternalsVisibleTo("Test.CMD")]

namespace Bot.Integration.Gitlab.Requests;

internal abstract class CommitRequest : RequestBase<CommitResponse>
{
    public override HttpMethod Method { get; } = HttpMethod.Post;
    public override string Url { get; } = "/api/v4/projects/{0}/repository/commits";

    [JsonPropertyName("branch")]
    public string Branch { get; } = string.Empty;
    [JsonPropertyName("commit_message")]
    public string CommitMessage { get; set; } = string.Empty;
    [JsonPropertyName("author_name")]
    public string? AuthorName { get; }
    [JsonPropertyName("author_email")]
    public string? AuthorEmail { get; }
    [JsonPropertyName("actions")]
    public CommitAction[] Actions { get; set; } = Array.Empty<CommitAction>();

    public CommitRequest(GitlabChatOptions options) : base(options)
    {
        Branch = options.BranchName;
        AuthorEmail = options.AuthorEmail;
        AuthorName = options.AuthorUsername;
        Url = string.Format(Url, options.Project.Replace("/", "%2F"));        
    }
}
