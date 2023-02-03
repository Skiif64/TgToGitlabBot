using Telegram.Bot;

namespace Bot.Integration.Telegram.Handlers.Base;

internal interface IHandler<T>
{
    Task HandleAsync(T data, ITelegramBotClient client, CancellationToken cancellationToken);
    bool CanHandle(T data);
}
