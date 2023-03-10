using Bot.Core.Entities;
using Bot.Integration.Telegram.Utils;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.Integration.Telegram.CommitFactories;

internal class ChannelCommitRequestFactory : TelegramChatCommitRequestFactory
{
    public ChannelCommitRequestFactory(Message message, ITelegramBotClient client) 
        : base(message, client)
    {
    }

    public override async Task<CommitRequest> CreateCommitRequestAsync(CancellationToken cancellationToken)
    {        
        var from = Message.AuthorSignature ?? "неизвестно";
        var builder = new StringBuilder();
        builder.AppendLine(Message.Caption ?? string.Empty);
        builder.Append("из: ");
        builder.AppendLine(Message.Chat.Title);
        builder.Append("от: ");
        builder.AppendLine(from);
        var message = builder.ToString();        
        
        if (message.StartsWith('\n'))
            message = message.Substring(1);
        var content = await DownloadFileAsync(cancellationToken);
        return new CommitRequest
        {
            From = from,
            Message= message,
            Content= content,
            FileName = Message.Document?.FileName ?? throw new ArgumentNullException(nameof(Message.Document)),
            FromChatId = Message.Chat.Id
        };
    }    
}
