using Bot.Integration.Telegram.Handlers;
using Bot.Integration.Telegram.Handlers.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Bot.Integration.Telegram;

public static class DependencyInjection
{
    public static IServiceCollection AddTelegramBot(this IServiceCollection services)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        services.AddSingleton<TelegramBotClientOptions>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<TelegramBotOptions>>().Value;
            if (!string.IsNullOrWhiteSpace(options.BaseUrl))
                return new TelegramBotClientOptions(
                token: options.BotToken,
                baseUrl: options.BaseUrl
                );
            else
                return new TelegramBotClientOptions(
                            token: options.BotToken
                            );
        });

        services.AddTransient<ITelegramBotClient, TelegramBotClientWrapper>();
        services.AddSingleton<IUpdateHandler, TelegramBotUpdateHandler>();
        services.AddHostedService<TelegramBot>();
        services.AddTransient<IHandler<Message>, MessageWithDocumentHandler>();
        services.AddTransient<IHandler<Message>, StatusCommandHandler>();
        return services;
    }
}
