using Bot.Core.Options;
using Bot.Integration.Gitlab.Primitives;
using Bot.Integration.Gitlab.Requests.Base;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

[assembly: InternalsVisibleTo("Test.CMD")]

namespace Bot.Integration.Gitlab.Requests;

internal abstract class CommitRequest : ProjectRequest
{
    public override HttpMethod Method { get; } = HttpMethod.Post;
    public override string Url { get; } = "/api/v4/projects/{0}/repository/commits"; 

    [JsonPropertyName("branch")]
    public string Branch { get;  } = string.Empty;
    [JsonPropertyName("commit_message")]
    public string CommitMessage { get; set; } = string.Empty;
    [JsonPropertyName("author_name")]
    public string? AuthorName { get;  }
    [JsonPropertyName("author_email")]
    public string? AuthorEmail { get;  }
    [JsonPropertyName("actions")]
    public CommitActionDto[] Actions { get; set; } = Array.Empty<CommitActionDto>();

    public CommitRequest(GitLabOptions options) : base(options)
    {
        Branch = options.BranchName;
        AuthorEmail = options.AuthorEmail;
        AuthorName = options.AuthorUsername;
        Url = string.Format(Url, options.ProjectNamespace+"%2F"+options.ProjectName);        
    }
}
