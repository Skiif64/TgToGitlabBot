namespace Bot.Integration.Gitlab.Abstractions
{
    public interface IGitlabRequest
    {
        HttpMethod Method { get; }
        string Url { get; }
        HttpContent Content { get; }
        string? AccessToken { get; }
    }
}