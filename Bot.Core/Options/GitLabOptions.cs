namespace Bot.Core.Options;

public class GitLabOptions
{
    public string ProjectPath{ get; init; } = string.Empty; //TODO: separate project path to namespace and project name
    public string BranchName { get; init; } = string.Empty;
    public string AuthorUsername { get; init; } = string.Empty;  //TODO: Maybe remove  
    public string AuthorEmail { get; init; } = string.Empty;
    public string? AccessToken { get; init; }
}
