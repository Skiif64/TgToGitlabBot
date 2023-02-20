using Bot.Integration.Git.GitCommands.Base;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;

namespace Bot.Integration.Git.GitCommands.Push;

internal class PushCommand : IGitCommand
{
    public string Branch { get; }
    public CredentialsHandler Credentials { get; }
    public IRepository Repository { get; }
    public PushCommand(IRepository repository, CredentialsHandler credentials, string branch)
    {
        Repository = repository;
        Credentials = credentials;
        Branch = branch;
    }
}
