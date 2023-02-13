using Bot.Core.ResultObject;
using LibGit2Sharp;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Bot.Git.Tests")]
namespace Bot.Integration.Git.GitCommands;

internal interface IGitCommand
{
    bool Execute(IRepository repository);
}
