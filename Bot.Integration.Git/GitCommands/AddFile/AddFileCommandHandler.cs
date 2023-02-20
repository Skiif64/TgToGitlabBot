using Bot.Integration.Git.GitCommands.Base;

namespace Bot.Integration.Git.GitCommands.AddFile;

internal class AddFileCommandHandler : IGitCommandHandler<AddFileCommand>
{
    public Task Handle(AddFileCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
