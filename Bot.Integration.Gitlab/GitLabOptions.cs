namespace Bot.Integration.Gitlab;

public class GitLabOptions
{
    public const string Path = "Gitlab";    
    public string Project { get; init; } = string.Empty;
    public string BranchName { get; init; } = string.Empty;
    public string AuthorUsername { get; init; } = string.Empty;  //TODO: Maybe remove  
    public string AuthorEmail { get; init; } = string.Empty;
    public string? AccessToken { get; init; }
}
