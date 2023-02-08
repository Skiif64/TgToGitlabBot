using Bot.Core.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
    private readonly TelegramBotOptions _options;
    public TelegramBot(ITelegramBotClient client,
        IUpdateHandler updateHandler,
        ILogger<ITelegramBot> logger,
        ReceiverOptions receiverOptions, IOptions<TelegramBotOptions> options)
    {
        _client = client;
        _updateHandler = updateHandler;
        _logger = logger;
        _receiverOptions = receiverOptions;
        _options = options.Value;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {       
        if (_options.UseWebhook)
            return StartWebhookAsync(cancellationToken);
        else
            return StartPollingAsync(cancellationToken);
    }

    public async Task StartPollingAsync(CancellationToken cancellationToken)
    {
        await _client.DeleteWebhookAsync(cancellationToken: cancellationToken);
        _client.StartReceiving(
             _updateHandler,
             _receiverOptions,
             cancellationToken);
        var me = await _client.GetMeAsync(cancellationToken);
        _logger.LogInformation($"Bot @{me.Username} is started in long polling mode");
    }

    public async Task StartWebhookAsync(CancellationToken cancellationToken)
    {
        if (!_options.UseWebhook)
            throw new InvalidOperationException("Cannot set webhook.Bot not in webhook mode");

        if (_options.WebhookUrl is null)
            throw new ArgumentException("Webhook is not set");
        await _client.SetWebhookAsync(
            url: _options.WebhookUrl,
            cancellationToken: cancellationToken
            );
        var me = await _client.GetMeAsync(cancellationToken);
        _logger.LogInformation($"Bot @{me.Username} is started in webhook mode");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if(_options.UseWebhook)
            await _client.DeleteWebhookAsync(cancellationToken: cancellationToken);
        //TODO: Do a normal stopping
    }
}
