using Bot.Integration.Gitlab.Requests.Base;

namespace Bot.Integration.Gitlab.Abstractions;

public interface IGitlabClient
{
    Task<bool> SendAsync(IGitlabRequest request, CancellationToken cancellationToken);
}
