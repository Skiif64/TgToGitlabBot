using Bot.Core.Entities;
using Bot.Integration.Telegram.Formatters;
using Bot.Integration.Telegram.Utils;
using System.Text;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.Integration.Telegram.CommitFactories;

internal class ChannelCommitRequestFactory : TelegramChatCommitRequestFactory
{
    private const string Pattern = @"_[vV]\d+(_\d+)*.+$";
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
        var filename = FormatFileName(Message.Document?.FileName);
        return new CommitRequest
        {
            From = from,
            Message= message,
            Content= content,
            FileName = filename,
            FromChatId = Message.Chat.Id
        };
    }

    private string FormatFileName(string? message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentNullException(nameof(message));

        var extension = Path.GetExtension(message);
        var filename = Path.GetFileNameWithoutExtension(message);
        var result = Regex.Replace(filename, Pattern, "");
        return result + extension;
    }
}
