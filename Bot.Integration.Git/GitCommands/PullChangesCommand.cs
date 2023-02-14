using LibGit2Sharp;
using LibGit2Sharp.Handlers;

namespace Bot.Integration.Git.GitCommands;

internal class PullChangesCommand : IGitCommand
{
    private readonly Signature _signature;
    private readonly CredentialsHandler _credentials;

    public PullChangesCommand(Signature signature, CredentialsHandler credentials)
    {
        _signature = signature;
        _credentials = credentials;
    }

    public void Execute(IRepository repository)
    {
        var options = new PullOptions
        {
            FetchOptions = new FetchOptions
            {
                CredentialsProvider = _credentials
            },
            MergeOptions = new MergeOptions
            {
                FileConflictStrategy = CheckoutFileConflictStrategy.Merge                
            }

        };
        Commands.Pull((Repository)repository, _signature, options);        
    }
}
