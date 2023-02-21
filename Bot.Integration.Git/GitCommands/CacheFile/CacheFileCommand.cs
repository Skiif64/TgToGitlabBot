using Bot.Integration.Git.GitCommands.Base;

namespace Bot.Integration.Git.GitCommands.CacheFile;

internal class CacheFileCommand : IGitCommand<string>
{
    public string Filepath { get; }
    public CacheFileCommand(string filepath)
    {
        if(string.IsNullOrWhiteSpace(filepath))
            throw new ArgumentNullException(nameof(filepath));
        Filepath = filepath;
    }
}
