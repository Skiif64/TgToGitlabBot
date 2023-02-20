using Bot.Integration.Git.GitCommands.Base;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;

namespace Bot.Integration.Git.GitCommands.PullChanges;

internal class PullChangesCommand : IGitCommand
{
    public Signature Signature { get; }
    public CredentialsHandler Credentials { get; }
    public IRepository Repository { get; }
    public PullChangesCommand(IRepository repository, Signature signature, CredentialsHandler credentials)
    {
        Signature = signature;
        Credentials = credentials;
        Repository = repository;
    }
}
