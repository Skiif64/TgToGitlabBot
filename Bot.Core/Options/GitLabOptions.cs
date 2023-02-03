namespace Bot.Core.Options;

public class GitLabOptions
{
    public string ProjectNamespace { get; init; } = string.Empty;
    public string ProjectName { get; init; } = string.Empty;
    public string BranchName { get; init; } = string.Empty;
    public string AuthorUsername { get; init; } = string.Empty;  //TODO: Maybe remove  
    public string AuthorEmail { get; init; } = string.Empty;
    public string? AccessToken { get; init; }
}
