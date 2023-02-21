using Bot.Core.Abstractions;
using Bot.Core.Entities;
using Bot.Core.Exceptions;
using Bot.Core.ResultObject;
using Bot.Integration.Telegram.Handlers.Base;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
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
        using var content = await DownloadFileAsync(client, document, cancellationToken);
        string message;
        string? from;
        if (data.Chat.Type is ChatType.Channel)
        {
            from = data.AuthorSignature;
            message = data.Caption ?? string.Empty;
            message += $"\nиз: {data.Chat.Title}";
        }
        else
        {
            from = data.From!.FirstName + " " + data.From.LastName;
            message = $"{document.FileName} от {from}";
        }

        message += $"\nот: {from}";
        if (message.StartsWith('\n'))
            message = message.Substring(1);

        var commitInfo = new CommitInfo
        {
            From = from ?? "unknown user",
            FromChatId = data.Chat.Id,
            Content = content,
            FileName = document.FileName!,
            Message = message
        };
        var result = await _gitlabService.CommitFileAndPushAsync(commitInfo, cancellationToken);
        if (result.Success)
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
            if (result is ErrorResult error)
            {
                await HandleErrorAsync(data, client, commitInfo, error, cancellationToken);
            }
            else
            {
                throw new InvalidOperationException("Unknown behavior");
            }
        }
    }

    private static Task HandleErrorAsync(Message data, ITelegramBotClient client, CommitInfo commitInfo, ErrorResult error, CancellationToken cancellationToken)
    {
        return error.Exception switch
        {
            ConfigurationNotSetException => client.SendTextMessageAsync(
            chatId: data.Chat.Id,
            text: $"Произошла ошибка при передаче файла {commitInfo.FileName}." +
            $"\nДля данного чата ({data.Chat.Id}) не задана конфигурация.",
            replyToMessageId: data.MessageId,
            cancellationToken: cancellationToken
                ),
            GitException ex => client.SendTextMessageAsync(
            chatId: data.Chat.Id,
            text: $"Произошла ошибка при передаче файла {commitInfo.FileName}." +
            $"\nОшибка Git: {ex.Message}",
            replyToMessageId: data.MessageId,
            cancellationToken: cancellationToken
                ),
            _ => client.SendTextMessageAsync(
                 chatId: data.Chat.Id,
                 text: $"Произошла ошибка при передаче файла {commitInfo.FileName}.",
                 replyToMessageId: data.MessageId,
                 cancellationToken: cancellationToken
                     )
        };
    }

    private static async Task<Stream> DownloadFileAsync(ITelegramBotClient client, Document document, CancellationToken cancellationToken)
    {
        var fileInfo = await client.GetFileAsync(document.FileId, cancellationToken);
        if (fileInfo.FilePath is null)
            throw new ApiRequestException("File path is empty", 400);
        if (client.LocalBotServer)
        {
            var fileStream = new FileStream(fileInfo.FilePath, FileMode.Open);
            return fileStream;
        }
        else
        {
            var stream = new MemoryStream();
            await client.DownloadFileAsync(fileInfo.FilePath, stream, cancellationToken);
            stream.Position = 0;
            return stream;
        }

    }
}
