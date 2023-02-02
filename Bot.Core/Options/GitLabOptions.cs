namespace Bot.Core.Options;

public class GitLabOptions
{
    public string ProjectPath{ get; init; } = string.Empty;
    public string BranchName { get; init; } = string.Empty;
    public string AuthorUsername { get; init; } = string.Empty;    
    public string AuthorEmail { get; init; } = string.Empty;
    public string? ProjectAccesToken { get; init; }
}
