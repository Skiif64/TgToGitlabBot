using Bot.Integration.Gitlab.Requests.Base;

namespace Bot.Integration.Gitlab.Abstractions;

public interface IGitlabClient
{
    Task<TResponse> SendAsync<TResponse>(IGitlabRequest<TResponse> request, CancellationToken cancellationToken);
}
