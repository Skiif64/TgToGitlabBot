using Bot.Integration.Git.GitCommands.Base;
using LibGit2Sharp;

namespace Bot.Integration.Git.GitCommands.PullChanges;

internal class PullChangesCommandHandler : IGitCommandHandler<PullChangesCommand>
{
    public Task Handle(PullChangesCommand request, CancellationToken cancellationToken)
    {
        var options = new PullOptions
        {
            FetchOptions = new FetchOptions
            {
                CredentialsProvider = request.Credentials
            },
            MergeOptions = new MergeOptions
            {
                FileConflictStrategy = CheckoutFileConflictStrategy.Merge
            }

        };
        Commands.Pull((Repository)request.Repository, request.Signature, options);
        return Task.CompletedTask;
    }
}
