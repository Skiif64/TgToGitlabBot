namespace Bot.Core.Abstractions;

public interface IConfigurationChecker
{
    bool Exists(long chatId);
}
