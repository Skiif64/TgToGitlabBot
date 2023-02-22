namespace Bot.Core.Errors;

public class GitError : Error
{
    public GitError() : base($"Ошибка Git'а при передаче файла")
    {
    }
}
