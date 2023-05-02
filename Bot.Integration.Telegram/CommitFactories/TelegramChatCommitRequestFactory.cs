using Bot.Core.Entities;
using Bot.Integration.Telegram.Handlers;
using Bot.Integration.Telegram.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.Integration.Telegram.CommitFactories
{
    internal abstract class TelegramChatCommitRequestFactory : ICommitRequestFactory
    {
        protected readonly Message Message;
        protected readonly ITelegramBotClient Client;
        public TelegramChatCommitRequestFactory(Message message, ITelegramBotClient client)
        {
            Message = message;
            Client = client;
        }
        public abstract Task<CommitRequest> CreateCommitRequestAsync(CancellationToken cancellationToken);

        protected internal virtual async Task<Stream> DownloadFileAsync(CancellationToken cancellationToken)
        {
            var document = Message.Document!;
            var fileInfo = await Client.GetFileAsync(document.FileId, cancellationToken);
            if (fileInfo.FilePath is null)
                throw new ArgumentNullException(nameof(fileInfo.FilePath));

            return await FileDownloader.DownloadFileAsync(fileInfo.FilePath, Client, cancellationToken);
        }
    }
}
