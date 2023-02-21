using Bot.Integration.Git.GitCommands.Base;
using LibGit2Sharp;

namespace Bot.Integration.Git.GitCommands.StageAndCommit;

internal class StageAndCommitCommand : IGitCommand<Commit>
{
    public string Filepath { get; }
    public string CommitMessage { get; }
    public Signature Signature { get; }
    public IRepository Repository { get; }

    public StageAndCommitCommand(IRepository repository, string filepath, string commitMessage, Signature signature)
    {
        Repository = repository;
        Filepath = filepath;
        CommitMessage = commitMessage;
        Signature = signature;
    }
}
