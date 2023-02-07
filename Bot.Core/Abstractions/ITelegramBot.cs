namespace Bot.Core.Abstractions;

public interface ITelegramBot
{
    Task StartPollingAsync(CancellationToken cancellationToken);
    Task StartWebhookAsync(CancellationToken cancellationToken);
}
