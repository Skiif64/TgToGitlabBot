namespace Bot.Integration.Gitlab.Abstractions;

public interface IExceptionParser
{
    Task<Exception> ParseAsync(HttpResponseMessage response, CancellationToken cancellationToken);
}
