namespace Bot.Integration.Git;

internal class GitOptions
{
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string AccessToken { get; init; } = string.Empty;
    public string Branch { get; init; } = string.Empty;
    public string Url { get; init; } = string.Empty;
    public string LocalPath { get; init; } = string.Empty;
}
