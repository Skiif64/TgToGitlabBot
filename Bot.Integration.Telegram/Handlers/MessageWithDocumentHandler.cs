using Bot.Core.Abstractions;
using Bot.Core.Entities;
using Bot.Core.Exceptions;
using Bot.Core.ResultObject;
using Bot.Integration.Telegram.CommitFactories;
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
        ICommitRequestFactory commitFactory;

        if (data.Chat.Type is ChatType.Channel)
            commitFactory = new ChannelCommitRequestFactory(data, client);
        else
            commitFactory = new GroupCommitRequestFactory(data, client);

        using var request = await commitFactory.CreateCommitRequestAsync(cancellationToken);
        var result = await _gitlabService.CommitFileAndPushAsync(request, cancellationToken);
        if (result.Success)
        {
            await client.SendTextMessageAsync(
                chatId: data.Chat.Id,
                text: $"Файл {request.FileName} успешно отправлен!",
                replyToMessageId: data.MessageId,
                cancellationToken: cancellationToken
                );
        }
        else
        {
            if (result is ErrorResult error)
            {
                await HandleErrorAsync(data, client, request, error, cancellationToken);
            }
            else
            {
                throw new InvalidOperationException("Unknown behavior");
            }
        }
    }

    private static Task HandleErrorAsync(Message data, ITelegramBotClient client, CommitRequest commitInfo, ErrorResult error, CancellationToken cancellationToken)
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
}
