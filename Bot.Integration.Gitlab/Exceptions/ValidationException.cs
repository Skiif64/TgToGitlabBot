namespace Bot.Integration.Gitlab.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(string exceptionMessage) : base(exceptionMessage)
    {

    }
}
