using Bot.Integration.Gitlab.Contracts;
using Bot.Integration.Gitlab.Requests.Base;

namespace Bot.Integration.Gitlab.Abstractions;

public interface IGitlabClient
{
    Task<CreateFileResponse> SendAsync(IGitlabRequest request, CancellationToken cancellationToken);
}
