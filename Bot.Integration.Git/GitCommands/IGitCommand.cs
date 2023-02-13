using LibGit2Sharp;

namespace Bot.Integration.Git.GitCommands;

internal interface IGitCommand
{
    bool Execute(IRepository repository);
}
