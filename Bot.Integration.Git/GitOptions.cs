namespace Bot.Integration.Git;

public class GitOptions
{
    public const string Path = "GitOptions";
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public Dictionary<string, GitOptionsSection> ChatOptions { get; init; } = new();
}

public class GitOptionsSection
{
    public string AccessToken { get; init; } = string.Empty;
    public string Branch { get; init; } = string.Empty;
    public string Url { get; init; } = string.Empty;
    public string LocalPath { get; init; } = string.Empty;
    public string? FilePath { get; init; }
}
