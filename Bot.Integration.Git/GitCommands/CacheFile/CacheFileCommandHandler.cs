using Bot.Integration.Git.GitCommands.Base;

namespace Bot.Integration.Git.GitCommands.CacheFile;

internal class CacheFileCommandHandler : IGitCommandHandler<CacheFileCommand, string>
{
    private const string EXTENSION = ".cached";
    public async Task<string> Handle(CacheFileCommand request, CancellationToken cancellationToken)
    {
        var cachedFilepath = request.Filepath + EXTENSION;

        await using var sourceFileStream = File.OpenRead(request.Filepath);
        await using var destFileStream = File.Create(cachedFilepath);
        await sourceFileStream.CopyToAsync(destFileStream, 1024, cancellationToken);
        return cachedFilepath;
    }
}
