using Bot.Core.Entities;

namespace Bot.Core.Abstractions;

public interface IGitlabService
{
    Task<bool> CommitFileAsync(CommitInfo file, CancellationToken cancellationToken = default);
}
