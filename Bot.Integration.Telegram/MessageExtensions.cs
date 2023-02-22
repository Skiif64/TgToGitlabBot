using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.Integration.Telegram;

internal static class MessageExtensions
{
    public static async Task ReplyAsync(this Message message,
                                        ITelegramBotClient client,
                                        string text,
                                        CancellationToken cancellationToken) =>    
        await client.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: text,
            replyToMessageId: message.MessageId,
            cancellationToken: cancellationToken);

    public static async Task AnswerAsync(this Message message,
                                        ITelegramBotClient client,
                                        string text,
                                        CancellationToken cancellationToken) =>
        await client.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: text,            
            cancellationToken: cancellationToken);

}
