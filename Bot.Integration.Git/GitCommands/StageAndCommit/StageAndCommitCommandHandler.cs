using Bot.Integration.Git.GitCommands.Base;
using LibGit2Sharp;

namespace Bot.Integration.Git.GitCommands.StageAndCommit;

internal class StageAndCommitCommandHandler : IGitCommandHandler<StageAndCommitCommand, Commit>
{
    public Task<Commit> Handle(StageAndCommitCommand request, CancellationToken cancellationToken)
    {
        Commands.Stage(request.Repository, request.Filepath);
        var commit = request.Repository.Commit(request.CommitMessage, request.Signature, request.Signature);
        return Task.FromResult(commit);
    }
}
