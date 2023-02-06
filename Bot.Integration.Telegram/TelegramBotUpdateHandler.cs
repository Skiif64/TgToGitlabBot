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
    private readonly ReceiverOptions _receiverOptions;

    public TelegramBotUpdateHandler(ILogger<TelegramBotUpdateHandler> logger,
        IServiceProvider serviceProvider,
        ReceiverOptions receiverOptions)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _receiverOptions = receiverOptions;
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var message = exception switch
        {            
            ApiRequestException api => $"Telegram API error. Error code: {api.ErrorCode}. Message: {api.Message}",
            _ => exception.Message
        };        
        _logger.LogError(message);              
        return Task.CompletedTask;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {     
        var action = update.Type switch
        {
            UpdateType.Message => OnMessageRecieved(update.Message!, botClient, cancellationToken),
            _ => throw new NotSupportedException($"Update type {update.Type} is not supported.")
        };        
        await action;
    }

    private async Task OnMessageRecieved(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        var handler = _serviceProvider
            .GetServices<IHandler<Message>>()
            .FirstOrDefault(h => h.CanHandle(message));
        if (handler == null)
            return;
        await handler.HandleAsync(message, botClient, cancellationToken);
    }
}
