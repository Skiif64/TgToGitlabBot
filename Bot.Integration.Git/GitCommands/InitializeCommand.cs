using LibGit2Sharp;
using LibGit2Sharp.Handlers;

namespace Bot.Integration.Git.GitCommands;

internal class InitializeCommand : IGitCommand
{
    private readonly GitOptionsSection _optionsSection;
    private readonly CredentialsHandler _credentials;

    public InitializeCommand(GitOptionsSection optionsSection, CredentialsHandler credentials)
    {
        _optionsSection = optionsSection;
        _credentials = credentials;
    }
    public bool Execute(IRepository repository)
    {        
        if (Repository.IsValid(_optionsSection.LocalPath))
            return true;
        
        if (!Directory.Exists(_optionsSection.LocalPath))
            Directory.CreateDirectory(_optionsSection.LocalPath);
        Repository.Clone(_optionsSection.Url, _optionsSection.LocalPath, new CloneOptions
        {
            BranchName = _optionsSection.Branch,
            CredentialsProvider = _credentials
        });
        return true;
    }
}
