using Bot.Integration.Git.GitCommands.Base;
using LibGit2Sharp;

namespace Bot.Integration.Git.GitCommands.Rollback;

internal class RollbackCommandHandler : IGitCommandHandler<RollbackCommand>
{
    public async Task Handle(RollbackCommand request, CancellationToken cancellationToken)
    {
        if (request.CachedFilepath != null)
        {
            await ReturnCachedToOriginalPositionAsync(request.Filepath, request.CachedFilepath, cancellationToken);
            File.Delete(request.CachedFilepath);

            if (!request.EmptyCommit)
                Commands.Unstage(request.Repository, request.CachedFilepath);
        }        
    }

    private static async Task ReturnCachedToOriginalPositionAsync(string originalFilepath, string cachedFilepath,
        CancellationToken cancellationToken)
    {
        await using var cachedFileStream = File.OpenRead(cachedFilepath);
        await using var destFileStream = File.Create(originalFilepath);
        await cachedFileStream.CopyToAsync(destFileStream, 1024, cancellationToken);
        Console.WriteLine();
    }
}
