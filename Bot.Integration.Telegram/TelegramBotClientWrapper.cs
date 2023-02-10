using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Requests.Abstractions;

namespace Bot.Integration.Telegram;

internal class TelegramBotClientWrapper : ITelegramBotClient, IDisposable
{
    private readonly ITelegramBotClient _telegramClient;
    private readonly HttpClient _httpClient;
    private bool _disposed;
    public bool LocalBotServer => _telegramClient.LocalBotServer;
    public long? BotId => _telegramClient.BotId;
    public TimeSpan Timeout
    {
        get => _telegramClient.Timeout;
        set => _telegramClient.Timeout = value;
    }
    public IExceptionParser ExceptionsParser
    {
        get => _telegramClient.ExceptionsParser;
        set => _telegramClient.ExceptionsParser = value;
    }

    public event AsyncEventHandler<ApiRequestEventArgs>? OnMakingApiRequest
    {
        add => _telegramClient.OnMakingApiRequest += value;
        remove => _telegramClient.OnMakingApiRequest -= value;
    }
    public event AsyncEventHandler<ApiResponseEventArgs>? OnApiResponseReceived
    {
        add => _telegramClient.OnApiResponseReceived += value;
        remove => _telegramClient.OnApiResponseReceived -= value;
    }

    public TelegramBotClientWrapper(TelegramBotClientOptions options)
    {
        _httpClient = new HttpClient();
        _telegramClient = new TelegramBotClient(options, _httpClient);
    }

    public void Dispose()
    {
        if (_disposed) return;
        _httpClient.Dispose();
        _disposed = true;
    }

    public Task DownloadFileAsync(string filePath, Stream destination, CancellationToken cancellationToken = default)
    {
        if (_disposed) throw new ObjectDisposedException(nameof(TelegramBotClientWrapper));
        return _telegramClient.DownloadFileAsync(filePath, destination, cancellationToken);
    }

    public Task<TResponse> MakeRequestAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        if (_disposed) throw new ObjectDisposedException(nameof(TelegramBotClientWrapper));
        return _telegramClient.MakeRequestAsync(request, cancellationToken);
    }

    public Task<bool> TestApiAsync(CancellationToken cancellationToken = default)
    {
        if (_disposed) throw new ObjectDisposedException(nameof(TelegramBotClientWrapper));
        return _telegramClient.TestApiAsync(cancellationToken);
    }
}
