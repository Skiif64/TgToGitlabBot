using Bot.Integration.Gitlab.Abstractions;
using Bot.Integration.Gitlab.Exceptions;
using System.Text.Json;

namespace Bot.Integration.Gitlab;

public class ExceptionParser : IExceptionParser
{
    //TODO: add descriptors
    public Exception Parse(string json)
    {
        var document = JsonDocument.Parse(json);
        var message = document.RootElement.GetProperty("message").GetString();
        return new ValidationException(message);
    }
}
