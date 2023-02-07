﻿using Bot.Core.Abstractions;
using Bot.Core.Entities;
using Bot.Core.Exceptions;
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
        var content = await DownloadFileAsync(client, document, cancellationToken);
        if (content.LongCount() > (long)int.MaxValue)
            throw new TooLargeException(nameof(content), content.LongCount(), int.MaxValue);
        var message = $"{document.FileName} from {data.From!.FirstName} {data.From!.LastName}";

        if (!string.IsNullOrWhiteSpace(data.Caption))
            message += $" message: {data.Caption}";
        
        var commitInfo = new CommitInfo
        {
            From = data.From!.Username!,
            FromChatId = data.Chat.Id,
            Content = content,
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

    private async Task<string> DownloadFileAsync(ITelegramBotClient client, Document document, CancellationToken cancellationToken)
    {
        if (client.LocalBotServer)
        {
            var fileInfo = await client.GetFileAsync(document.FileId, cancellationToken);
            await using (var fs = new FileStream(fileInfo.FilePath, FileMode.Open))
            {
                using (var br = new BinaryReader(fs))
                {
                    return Convert.ToBase64String(br.ReadBytes((int)fs.Length));
                }
            }
        }
        else
        {
            await using (var stream = new MemoryStream())
            {
                await client.GetInfoAndDownloadFileAsync(document.FileId, stream, cancellationToken);
                using (var br = new BinaryReader(stream))
                {
                    stream.Position = 0;
                    return Convert.ToBase64String(br.ReadBytes((int)stream.Length));
                }
            }
        }

    }
}
