using Bot.Integration.Telegram.Handlers.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Bot.Integration.Telegram;

internal class TelegramBotUpdateHandler : IUpdateHandler
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TelegramBotUpdateHandler> _logger;

    public TelegramBotUpdateHandler(ILogger<TelegramBotUpdateHandler> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var message = exception switch
        {
            ApiRequestException api => $"Telegram API error. Error code: {api.ErrorCode}. Message: {api.Message}",
            _ => exception.Message
        };
        _logger.LogError(message);
        //botClient.StartReceiving(this, _receiverOptions, cancellationToken);
        return Task.CompletedTask;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var action = update.Type switch
        {
            UpdateType.Message => OnMessageRecieved(update.Message!, botClient, cancellationToken),
            UpdateType.ChannelPost => OnMessageRecieved(update.ChannelPost!, botClient, cancellationToken),
            _ => throw new NotSupportedException($"Update type {update.Type} is not supported.")
        };
        await action;
    }

    private async Task OnMessageRecieved(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var handler = scope.ServiceProvider
            .GetServices<IHandler<Message>>()
            .FirstOrDefault(h => h.CanHandle(message));
        if (handler == null)
            return;
        await handler.HandleAsync(message, botClient, cancellationToken);
    }
}
