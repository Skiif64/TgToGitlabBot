using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace Bot.Integration.Telegram;

public static class DependencyInjection
{
    public static IServiceCollection AddTelegramBot(this IServiceCollection services, IConfiguration config)
    {
        var botToken = config.GetRequiredSection("TelegramBot__BotToken").Value;
        services.AddHostedService<TelegramBot>();
        services.AddSingleton<ITelegramBotClient, TelegramBotClient>(x => new TelegramBotClient(botToken));
        services.AddSingleton<IUpdateHandler, TelegramBotUpdateHandler>();
        return services;
    }
}
