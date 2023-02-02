namespace Bot.Core.Abstractions;

public interface ITelegramBot
{
    Task StartPollingAsync(CancellationToken cancellationToken);
}
