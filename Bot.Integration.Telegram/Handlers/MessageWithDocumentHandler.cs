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
        if(data == null || data.Document == null)
            return false;

        return true;
    }

    public async Task HandleAsync(Message data, ITelegramBotClient client, CancellationToken cancellationToken)
    {        
        var document = data.Document!;        
        using var ms = new MemoryStream();
        await client.GetInfoAndDownloadFileAsync(document.FileId, ms, cancellationToken);
        var message = string.Empty;

        if(data.Caption != null)
            message = $"{data.Caption}";
        else
            message = $"{document.FileName} from {data.From!.FirstName} {data.From!.LastName}";

        var commitInfo = new CommitInfo
        {
            From = data.From!.Username!,
            Content = ms,
            FileName = document.FileName!,
            Message = message
        };
        await _gitlabService.CommitFileAsync(commitInfo, cancellationToken);
        //TODO: Validation?
    }
}
