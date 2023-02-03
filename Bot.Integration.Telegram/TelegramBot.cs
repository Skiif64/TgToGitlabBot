using Bot.Core.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace Bot.Integration.Telegram;

internal class TelegramBot : ITelegramBot, IHostedService
{
    private readonly ITelegramBotClient _client;
    private readonly IUpdateHandler _updateHandler;
    private readonly ILogger<ITelegramBot> _logger;
    private readonly ReceiverOptions _receiverOptions;
    public TelegramBot(ITelegramBotClient client,
        IUpdateHandler updateHandler,
        ILogger<ITelegramBot> logger,
        ReceiverOptions receiverOptions)
    {
        _client = client;
        _updateHandler = updateHandler;
        _logger = logger;
        _receiverOptions = receiverOptions;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
       return StartPollingAsync(cancellationToken);
    }

    public async Task StartPollingAsync(CancellationToken cancellationToken)
    {
       _client.StartReceiving(
            _updateHandler,
            _receiverOptions,
            cancellationToken);
        var me = await _client.GetMeAsync(cancellationToken);
        _logger.LogInformation($"Bot {me.Username} is started");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {        
        return Task.CompletedTask; //TODO: Do a normal stopping
    }
}
