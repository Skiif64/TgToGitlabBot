using Bot.Integration.Git.GitCommands.Base;
using LibGit2Sharp.Handlers;

namespace Bot.Integration.Git.GitCommands.Initialize;

internal class InitializeCommand : IGitCommand
{
    public GitOptionsSection OptionsSection { get; }
    public CredentialsHandler Credentials { get; }
    public InitializeCommand(GitOptionsSection optionsSection, CredentialsHandler credentials)
    {
        OptionsSection = optionsSection;
        Credentials = credentials;
    }
}
