using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace Bot.Integration.Telegram;

public static class DependencyInjection
{
    public static IServiceCollection AddTelegramBot(this IServiceCollection services)
    {        
        services.AddSingleton<IUpdateHandler, TelegramBotUpdateHandler>();
        services.AddHostedService<TelegramBot>();        
        return services;
    }
}
