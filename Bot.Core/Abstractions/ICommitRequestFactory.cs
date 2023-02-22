using Bot.Core.Entities;

namespace Bot.Integration.Telegram.Handlers;

public interface ICommitRequestFactory
{
    Task<CommitRequest> CreateCommitRequestAsync(CancellationToken cancellationToken);
}