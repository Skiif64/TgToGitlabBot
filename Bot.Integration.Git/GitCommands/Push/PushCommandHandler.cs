using Bot.Integration.Git.GitCommands.Base;
using LibGit2Sharp;

namespace Bot.Integration.Git.GitCommands.Push;

internal class PushCommandHandler : IGitCommandHandler<PushCommand>
{
    public Task Handle(PushCommand request, CancellationToken cancellationToken)
    {
        var remote = request.Repository.Network.Remotes["origin"];
        if (remote == null)
            throw new ArgumentNullException(nameof(remote));
        request.Repository.Network.Push(remote, $@"refs/heads/{request.Branch}", new PushOptions
        {
            CredentialsProvider = request.Credentials
        });
        return Task.CompletedTask;
    }
}
