using System.IO;
using Telegram.Bot;

namespace Bot.Integration.Telegram.Utils;

internal class FileDownloader
{
    public static async Task<Stream> DownloadFileAsync(string filepath,
                                           ITelegramBotClient client,
                                           CancellationToken cancellationToken)
    {
        if (client.LocalBotServer)
        {
            var fileStream = new FileStream(filepath, FileMode.Open);
            return fileStream;
        }
        else
        {
            var stream = new MemoryStream();
            await client.DownloadFileAsync(filepath, stream, cancellationToken);
            stream.Position = 0;
            return stream;
        }
    }
}
