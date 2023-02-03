using Bot.Integration.Telegram.Handlers;
using Bot.Integration.Telegram.Handlers.Base;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Bot.Integration.Telegram;

public static class DependencyInjection
{
    public static IServiceCollection AddTelegramBot(this IServiceCollection services)
    {        
        services.AddSingleton<IUpdateHandler, TelegramBotUpdateHandler>();
        services.AddHostedService<TelegramBot>();
        services.AddTransient<IHandler<Message>, MessageWithDocumentHandler>();
        return services;
    }
}
