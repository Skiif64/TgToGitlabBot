using Bot.Core.Abstractions;
using Bot.Core.ResultObject;
using Bot.Integration.Telegram.CommitFactories;
using Bot.Integration.Telegram.Handlers.Base;
using Bot.Integration.Telegram.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Bot.Integration.Telegram.Handlers;

internal class MessageWithDocumentHandler : IHandler<Message>
{
    private readonly IGitlabService _gitlabService;
    private readonly ChatLogger _logger;

    public MessageWithDocumentHandler(IGitlabService gitlabService, ChatLogger logger)
    {
        _gitlabService = gitlabService;
        _logger = logger;
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
        await _logger.LogAnswerAsync(result, data, request, cancellationToken);
    }
}
