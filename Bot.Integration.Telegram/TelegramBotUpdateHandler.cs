using Bot.Core.Abstractions;
using Bot.Core.Entities;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Bot.Integration.Telegram;

internal class TelegramBotUpdateHandler : IUpdateHandler
{
    private readonly IGitlabService _gitlabService;
    private readonly ILogger<TelegramBotUpdateHandler> _logger;

    public TelegramBotUpdateHandler(IGitlabService gitlabService, ILogger<TelegramBotUpdateHandler> logger)
    {
        _gitlabService = gitlabService;
        _logger = logger;
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var message = exception switch
        {
            ApiRequestException api => $"Telegram API error. Error code: {api.ErrorCode}. Message: {api.Message}",
            _ => exception.ToString()
        };
        _logger.LogError(message);
        return Task.CompletedTask;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type != UpdateType.Message 
            || update is null 
            || update.Message is null 
            || update.Message.Document is null)
            return;

        var message = update.Message;
        var document = message.Document;
        var file = await botClient.GetFileAsync(document.FileId, cancellationToken);
        using var ms = new MemoryStream();
        await botClient.DownloadFileAsync(file.FilePath, ms, cancellationToken);
        var commitInfo = new CommitInfo
        {
            From = message.From.Username,
            Content = ms,
            FileName = document.FileName,
            Message = message.Text
        };
        await _gitlabService.CommitFileAsync(commitInfo, cancellationToken);  
    }
}
