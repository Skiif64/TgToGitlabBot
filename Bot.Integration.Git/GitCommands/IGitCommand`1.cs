using LibGit2Sharp;

namespace Bot.Integration.Git.GitCommands;

internal interface IGitCommand<TResult>
{
    TResult Execute(IRepository repository);
}
