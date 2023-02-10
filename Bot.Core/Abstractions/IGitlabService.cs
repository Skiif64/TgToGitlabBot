using Bot.Core.Entities;

namespace Bot.Core.Abstractions;

public interface IGitlabService
{
    Task<bool> CommitFileAndPushAsync(CommitInfo file, CancellationToken cancellationToken = default);
}
