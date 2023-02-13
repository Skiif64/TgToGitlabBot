namespace Bot.Integration.Telegram;

public class TelegramBotOptions
{
    public const string Path = "TelegramBot";
    public string BotToken { get; init; } = string.Empty;
    public string? BaseUrl { get; init; }   
    public string? WebhookUrl { get; init; }
}
