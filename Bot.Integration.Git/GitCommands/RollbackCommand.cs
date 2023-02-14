using LibGit2Sharp;

namespace Bot.Integration.Git.GitCommands;

internal class RollbackCommand : IGitCommand
{
    private readonly string _filepath;
    private readonly string? _cachedFilepath;
    private readonly bool _emptyCommit;
    public RollbackCommand(string filepath, string? cachedFilepath, bool emptyCommit)
    {
        _filepath = filepath;
        _cachedFilepath = cachedFilepath;
        _emptyCommit = emptyCommit;
    }
    public void Execute(IRepository repository)
    {
        if (_cachedFilepath != null)
        {
            ReturnCachedToOriginalPosition(_filepath, _cachedFilepath);
            File.Delete(_cachedFilepath);
        }
        if (!_emptyCommit)
            Commands.Unstage(repository, _filepath);
    }

    private void ReturnCachedToOriginalPosition(string originalFilepath, string cachedFilepath)
    {
        using var cachedFileStream = File.OpenRead(cachedFilepath);
        using var destFileStream = File.OpenWrite(originalFilepath);
        Span<byte> buffer = stackalloc byte[1024];
        int readed;
        while ((readed = cachedFileStream.Read(buffer)) > 0)
        {
            if (buffer.Length > readed)
                destFileStream.Write(buffer[..readed]);
            else
                destFileStream.Write(buffer);
        }
    }
}
