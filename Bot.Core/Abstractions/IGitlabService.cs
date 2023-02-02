using Bot.Core.Entities;

namespace Bot.Core.Abstractions;

public interface IGitlabService
{
    Task CommitFileAsync(CommitInfo file, CancellationToken cancellationToken = default);
}
