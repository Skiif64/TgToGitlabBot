using LibGit2Sharp;

namespace Bot.Integration.Git.GitCommands;

internal class CacheFileCommand : IGitCommand<string>
{
    private readonly string _filepath;

    public CacheFileCommand(string filepath)
    {
        _filepath = filepath;
    }

    public string Execute(IRepository repository)
    {
        var cachedFilepath = _filepath + ".cached";

        using var sourceFileStream = File.OpenRead(_filepath);
        using var destFileStream = File.Create(cachedFilepath);

        Span<byte> buffer = stackalloc byte[1024];
        int readed;
        while ((readed = sourceFileStream.Read(buffer)) > 0)
        {
            if (buffer.Length > readed)
                destFileStream.Write(buffer[..readed]);
            else
                destFileStream.Write(buffer);
        }
        return cachedFilepath;
    }
}
