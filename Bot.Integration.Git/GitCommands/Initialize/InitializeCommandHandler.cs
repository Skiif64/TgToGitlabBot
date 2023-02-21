using Bot.Integration.Git.GitCommands.Base;
using LibGit2Sharp;

namespace Bot.Integration.Git.GitCommands.Initialize;

internal class InitializeCommandHandler : IGitCommandHandler<InitializeCommand>
{
    public Task Handle(InitializeCommand request, CancellationToken cancellationToken)
    {
        if (Repository.IsValid(request.OptionsSection.LocalPath))
            return Task.CompletedTask;

        if (!Directory.Exists(request.OptionsSection.LocalPath))
            Directory.CreateDirectory(request.OptionsSection.LocalPath);
        Repository.Clone(request.OptionsSection.Url, request.OptionsSection.LocalPath, new CloneOptions
        {
            BranchName = request.OptionsSection.Branch,
            CredentialsProvider = request.Credentials
        });
        return Task.CompletedTask;
    }
}
