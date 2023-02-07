namespace Bot.Core.Exceptions;

public class TooLargeException : Exception
{
    public TooLargeException(string filename, long size, long maxAllowed)
        : base($"{filename} is too large. file size: {size}, max allowed: {maxAllowed}")
    {

    }
}
