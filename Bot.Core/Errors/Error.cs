namespace Bot.Core.Errors;

public abstract class Error
{
    public string Message { get; }

    protected Error(string message)
    {
        Message = message;
    }
}
