﻿namespace Bot.Integration.Gitlab;

public class GitLabOptions
{
    public const string Path = "Gitlab";
    public string BaseUrl { get; init; } = string.Empty;
    public Dictionary<string, GitlabChatOptions> ChatOptions { get; init; } = null!;
}

public class GitlabChatOptions
{
    public string Project { get; init; } = string.Empty;
    public string? DisplayProjectName { get; init; }
    public string BranchName { get; init; } = string.Empty;
    public string AuthorUsername { get; init; } = string.Empty;  //TODO: Maybe remove  
    public string AuthorEmail { get; init; } = string.Empty;
    public string? AccessToken { get; init; }
    public string? FilePath { get; init; }
}
