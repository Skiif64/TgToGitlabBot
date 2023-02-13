using Bot.Core.Entities;
using Bot.Core.ResultObject;

namespace Bot.Core.Abstractions;

public interface IGitlabService
{
    Task<Result<bool>> CommitFileAndPushAsync(CommitInfo file, CancellationToken cancellationToken = default);
}
