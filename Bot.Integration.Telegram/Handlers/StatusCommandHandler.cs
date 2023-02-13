using Bot.Core.Abstractions;
using Bot.Integration.Telegram.Handlers.Base;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.Integration.Telegram.Handlers;

internal class StatusCommandHandler : IHandler<Message>
{
    private readonly IConfigurationChecker _configurationChecker;

    public StatusCommandHandler(IConfigurationChecker configurationChecker)
    {
        _configurationChecker = configurationChecker;
    }

    public bool CanHandle(Message data)
    {
        if (data is null || data.Text is null || !data.Text.StartsWith("/status"))
            return false;

        return true;
    }

    public async Task HandleAsync(Message data, ITelegramBotClient client, CancellationToken cancellationToken)
    {
        var chatId = data.Chat.Id;

        if (_configurationChecker.Exists(chatId))
        {
            await client.SendTextMessageAsync(
                chatId,
                $"Текущий id чата: {chatId}, конфигурация имеется",
                cancellationToken: cancellationToken);
        }
        else
        {
            await client.SendTextMessageAsync(
                chatId,
                $"Текущий id чата: {chatId}, конфигурация отсутствует",
                cancellationToken: cancellationToken);
        }
    }
}
