using Bot.Integration.Git.GitCommands.Base;

namespace Bot.Integration.Git.GitCommands.AddFile;

internal class AddFileCommandHandler : IGitCommandHandler<AddFileCommand>
{
    public async Task Handle(AddFileCommand request, CancellationToken cancellationToken)
    {
        await using var fileStream = new FileStream(request.Filepath, FileMode.OpenOrCreate, FileAccess.Write);
        await request.ContentStream.CopyToAsync(fileStream, 1024, cancellationToken);
    }
}
