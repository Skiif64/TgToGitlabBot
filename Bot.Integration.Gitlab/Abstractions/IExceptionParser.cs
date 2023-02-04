namespace Bot.Integration.Gitlab.Abstractions;

public interface IExceptionParser
{
    Exception Parse(string json);
}
