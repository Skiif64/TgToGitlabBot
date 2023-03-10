using Bot.Core.Abstractions;
using Bot.Core.ResultObject;
using Bot.Integration.Telegram.CommitFactories;
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
        ICommitRequestFactory commitFactory;

        if (data.Chat.Type is ChatType.Channel)
            commitFactory = new ChannelCommitRequestFactory(data, client);
        else
            commitFactory = new GroupCommitRequestFactory(data, client);

        using var request = await commitFactory.CreateCommitRequestAsync(cancellationToken);
        var result = await _gitlabService.CommitFileAndPushAsync(request, cancellationToken);
        if (result.Success)
        {
            await data.ReplyAsync(client, $"Файл {request.FileName} успешно отправлен!", cancellationToken);            
        }
        else
        {
            if (result is ErrorResult error)
            {
                await data.ReplyAsync(client,
                    $"Произошла ошибка при передаче файла {request.FileName}. {error.Error.Message}",
                    cancellationToken);                
            }
            else
            {
                throw new InvalidOperationException("Unknown behavior");
            }
        }
    }
}
