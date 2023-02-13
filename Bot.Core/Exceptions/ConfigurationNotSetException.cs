namespace Bot.Core.Exceptions;

public class ConfigurationNotSetException : Exception
{
    public long ChatID { get; }
	public ConfigurationNotSetException(long chatId) : base($"Configuration for {chatId} is not set!")
	{

	}
}
