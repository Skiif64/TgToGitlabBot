using Bot.Core.Entities;
using Bot.Core.ResultObject;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;

namespace Bot.Integration.Telegram.Utils;
internal class ChatLogger
{
    private readonly ITelegramBotClient _client;
    private readonly TelegramBotOptions _options;

    public ChatLogger(ITelegramBotClient client, IOptionsSnapshot<TelegramBotOptions> options)
    {
        _client = client;
        _options = options.Value;
    }

    public async Task LogAnswerAsync(Result result,
                                     Message data,
                                     CommitRequest request,
                                     CancellationToken cancellationToken)
    {        
        if (result.Success)
        {
            await _client.SendTextMessageAsync(_options.LoggingChat,
                $"✔ Чат: {data.Chat.Title} | Файл {request.FileName} успешно отправлен!",
                cancellationToken: cancellationToken);            
        }
        else
        {
            if (result is ErrorResult error)
            {
                await _client.SendTextMessageAsync(_options.LoggingChat,
                $"❌ Чат: {data.Chat.Title} | Произошла ошибка при передаче файла {request.FileName}. {error.Error.Message}",
                cancellationToken: cancellationToken);
            }
            else
            {
                await _client.SendTextMessageAsync(_options.LoggingChat,
                $"❌ ❗ ❗ Чат: {data.Chat.Title} | Произошла ошибка при передаче файла {request.FileName}. Данной ошибки не должно существовать",
                cancellationToken: cancellationToken);                
                throw new InvalidOperationException("Unknown behavior");
            }
        }
    }
}
