namespace Bot.Integration.Gitlab.Exceptions;

public class AuthentificationException : Exception
{
    public AuthentificationException(string message, string description)
        : base($"{message} | description: {description}")
    {

    }
}
