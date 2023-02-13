using Bot.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace Bot.Integration.Telegram;

internal class TelegramBot : ITelegramBot, IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ITelegramBot> _logger;
    private readonly TelegramBotOptions _options;
    public TelegramBot(
       ILogger<ITelegramBot> logger,
       IOptions<TelegramBotOptions> options,
       IServiceProvider serviceProvider)
    {
        _logger = logger;
        _options = options.Value;
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return StartWebhookAsync(cancellationToken);
    }

    public Task StartPollingAsync(CancellationToken cancellationToken)
    {
        throw new NotSupportedException("Long polling no longer support");
    }

    public async Task StartWebhookAsync(CancellationToken cancellationToken)
    {
        var client = _serviceProvider.GetRequiredService<ITelegramBotClient>();

        if (_options.WebhookUrl is null)
            throw new ArgumentException("Webhook is not set");
        await client.SetWebhookAsync(
            url: _options.WebhookUrl,
            cancellationToken: cancellationToken
            );
        var me = await client.GetMeAsync(cancellationToken);
        _logger.LogInformation($"Bot @{me.Username} is started in webhook mode");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        var client = _serviceProvider.GetRequiredService<ITelegramBotClient>();
        await client.DeleteWebhookAsync(cancellationToken: cancellationToken);        
    }
}
