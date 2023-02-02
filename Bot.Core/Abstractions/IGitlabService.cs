using Bot.Core.Entities;

namespace Bot.Core.Abstractions;

public interface IGitlabService
{
    Task SendFileAsync(CommitInfo file, CancellationToken cancellationToken = default);
}
