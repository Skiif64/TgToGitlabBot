namespace Bot.Integration.Gitlab.Abstractions;

public interface IGitlabClient
{
    Task SendAsync(IGitlabRequest request, CancellationToken cancellationToken);
}
