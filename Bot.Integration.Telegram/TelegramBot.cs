using Bot.Core.Abstractions;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace Bot.Integration.Telegram;

//TODO: Make internal
public class TelegramBot : ITelegramBot, IHostedService
{
    private readonly ITelegramBotClient _client;
    private readonly IUpdateHandler _updateHandler;
    public TelegramBot(ITelegramBotClient client, IUpdateHandler updateHandler)
    {
        _client = client;
        _updateHandler = updateHandler;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
       return StartPollingAsync(cancellationToken);
    }

    public async Task StartPollingAsync(CancellationToken cancellationToken)
    {
       await Task.Factory.StartNew(() => _client.StartReceiving(
            _updateHandler,
            new ReceiverOptions 
            { 
                ThrowPendingUpdates = true, //TODO: set false
                AllowedUpdates = new[] {UpdateType.Message}
            },
            cancellationToken));
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask; //TODO: Do a normal stopping
    }
}
