namespace Bot.Core.Errors;

public class EmptyCommitError : Error
{
    public EmptyCommitError() : base("Пустой коммит, возможно в файле отсутствуют какие-либо изменения")
    {
    }
}
