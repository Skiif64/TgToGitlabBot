namespace Bot.Core.Errors;

public class ConfigurationNotSetError : Error
{
    public ConfigurationNotSetError(long chatId) : base($"Конфигурация для чата {chatId} не задана!")
    {
    }
}
