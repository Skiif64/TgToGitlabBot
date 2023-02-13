using LibGit2Sharp;

namespace Bot.Integration.Git.GitCommands;

internal class StageAndCommitCommand : IGitCommand
{
    private readonly string _filepath;
    private readonly string _commitMessage;
    private readonly Signature _signature;

    public StageAndCommitCommand(string filepath, string commitMessage, Signature signature)
    {
        _filepath = filepath;
        _commitMessage = commitMessage;
        _signature = signature;
    }
    public bool Execute(IRepository repository)
    {        
        Commands.Stage(repository, _filepath);
        var commit = repository.Commit(_commitMessage, _signature, _signature);
        return commit != null;
    }
}
