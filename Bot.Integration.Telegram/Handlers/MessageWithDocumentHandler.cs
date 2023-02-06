using Bot.Core.Abstractions;
using Bot.Core.Entities;
using Bot.Integration.Telegram.Handlers.Base;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.Integration.Telegram.Handlers;

internal class MessageWithDocumentHandler : IHandler<Message>
{
    private readonly IGitlabService _gitlabService;

    public MessageWithDocumentHandler(IGitlabService gitlabService)
    {
        _gitlabService = gitlabService;
    }

    public bool CanHandle(Message data)
    {
        if (data == null || data.Document == null)
            return false;

        return true;
    }

    public async Task HandleAsync(Message data, ITelegramBotClient client, CancellationToken cancellationToken)
    {
        var document = data.Document!;
        await using var stream = new MemoryStream();
        await client.GetInfoAndDownloadFileAsync(document.FileId, stream, cancellationToken);
        var message = $"{document.FileName} from {data.From!.FirstName} {data.From!.LastName}";

        if (!string.IsNullOrWhiteSpace(data.Caption))
            message += $" message: {data.Caption}";
        
        var commitInfo = new CommitInfo
        {
            From = data.From!.Username!,
            FromChatId = data.Chat.Id,
            Content = stream,
            FileName = document.FileName!,
            Message = message
        };
        var result = await _gitlabService.CommitFileAsync(commitInfo, cancellationToken);
        if (result)
        {
            await client.SendTextMessageAsync(
                chatId: data.Chat.Id,
                text: $"Файл {commitInfo.FileName} успешно отправлен!",
                replyToMessageId: data.MessageId,
                cancellationToken: cancellationToken
                );
        }
        else
        {
            await client.SendTextMessageAsync(
               chatId: data.Chat.Id,
               text: $"Произошла ошибка при передаче файла {commitInfo.FileName}",
               replyToMessageId: data.MessageId,
               cancellationToken: cancellationToken
                );
        }
    }
}
