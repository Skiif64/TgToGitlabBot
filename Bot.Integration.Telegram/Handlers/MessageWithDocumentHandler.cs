using Bot.Core.Abstractions;
using Bot.Core.Entities;
using Bot.Core.Exceptions;
using Bot.Integration.Telegram.Handlers.Base;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

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
        string message;
        string from;
        if (data.Chat.Type is ChatType.Channel)
        {
            from = data.AuthorSignature;
            message = data.Caption;
            message += $"\nиз: {data.Chat.Title}";
        }
        else
        {
            from = data.From.FirstName + " " + data.From.LastName;
            message = $"{document.FileName} от {from}";
        }

        message += $"\nот: {from}";
        if (message.StartsWith('\n'))
            message = message.Substring(1);

        var commitInfo = new CommitInfo
        {
            From = from,
            FromChatId = data.Chat.Id,
            Content = content.Content,
            ContentType = content.ContentType,
            FileName = document.FileName!,
            Message = message
        };
        var result = await _gitlabService.CommitFileAndPushAsync(commitInfo, cancellationToken);
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

    private async Task<(Stream Content, string ContentType)> DownloadFileAsync(ITelegramBotClient client, Document document, CancellationToken cancellationToken)
    {
        if (client.LocalBotServer)
        {
            var fileInfo = await client.GetFileAsync(document.FileId, cancellationToken);
            var fs = new FileStream(fileInfo.FilePath, FileMode.Open);
            if (fs.Length >= 200_000_000)
                throw new TooLargeException(nameof(fs), fs.Length, 200_000_000);
            return (fs, "text");

        }
        else
        {
            await using (var stream = new MemoryStream())
            {
                await client.GetInfoAndDownloadFileAsync(document.FileId, stream, cancellationToken);
                if (stream.Length >= 200_000_000)
                    throw new TooLargeException(nameof(stream), stream.Length, 200_000_000);

                stream.Position = 0;
                return (stream, "text");

            }
        }

    }

    private (string Content, string ContentType) GetFileStream(Stream stream)
    {
        using (var br = new BinaryReader(stream))
        {
            return (Convert.ToBase64String(br.ReadBytes((int)stream.Length)), "base64");
        }
        //using (var detector = new FileTypeDetector(stream))
        //{
        //    if (detector.IsBinary())
        //    {
        //        using (var br = new BinaryReader(stream))
        //        {
        //            return (Convert.ToBase64String(br.ReadBytes((int)stream.Length)), "base64");
        //        }
        //    }
        //    else
        //    {
        //        var encoding = Encoding.GetEncoding("windows-1251");
        //        if (detector.IsUtf8Encoded())
        //            encoding = Encoding.UTF8;

        //        using (var sr = new StreamReader(stream, encoding))
        //        {
        //            return (sr.ReadToEnd(), "text");
        //        }
        //    }
        //}
    }
}
