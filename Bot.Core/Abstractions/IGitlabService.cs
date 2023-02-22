using Bot.Core.Entities;
using Bot.Core.ResultObject;

namespace Bot.Core.Abstractions;

public interface IGitlabService
{
    Task<Result> CommitFileAndPushAsync(CommitRequest file, CancellationToken cancellationToken = default);
}
