using Bot.Integration.Git.GitCommands.Base;
using LibGit2Sharp;

namespace Bot.Integration.Git.GitCommands.Rollback;

internal class RollbackCommand : IGitCommand
{
    public string Filepath { get; }
    public string? CachedFilepath { get; }
    public bool EmptyCommit { get; }
    public IRepository Repository { get; }

    public RollbackCommand(IRepository repository, string filepath, string? cachedFilepath, bool emptyCommit)
    {
        Repository = repository;
        Filepath = filepath;
        CachedFilepath = cachedFilepath;
        EmptyCommit = emptyCommit;
    }
}
