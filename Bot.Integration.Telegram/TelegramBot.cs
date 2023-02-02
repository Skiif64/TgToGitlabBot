using Bot.Core.Abstractions;
using Bot.Core.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace Bot.Integration.Telegram;

//TODO: Make internal
public class TelegramBot : ITelegramBot, IHostedService
{
    private readonly ITelegramBotClient _client;
    private readonly IUpdateHandler _updateHandler;
    public TelegramBot(TelegramBotOptions options, IUpdateHandler updateHandler)
    {
        _client = new TelegramBotClient(options.BotToken);        
        _updateHandler = updateHandler;
    }

    public TelegramBot(IOptions<TelegramBotOptions> options, IUpdateHandler updateHandler) 
        : this(options.Value, updateHandler)
    {

    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
       return StartPollingAsync(cancellationToken);
    }

    public Task StartPollingAsync(CancellationToken cancellationToken)
    {
       _client.StartReceiving(
            _updateHandler,
            new ReceiverOptions 
            { 
                ThrowPendingUpdates = false, //TODO: set false
                //AllowedUpdates = new[] {UpdateType.Message}
                
            },
            cancellationToken);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask; //TODO: Do a normal stopping
    }
}
