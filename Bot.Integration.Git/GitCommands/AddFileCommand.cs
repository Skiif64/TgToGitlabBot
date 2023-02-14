using Bot.Core.Entities;
using LibGit2Sharp;

namespace Bot.Integration.Git.GitCommands;

internal class AddFileCommand : IGitCommand
{
    private readonly Stream _content;
    private readonly string _filepath;

    public AddFileCommand(Stream content, string filepath)
    {
        _content = content;
        _filepath = filepath;
    }
    public void Execute(IRepository repository)
    {
        if (_content is null)
            throw new ArgumentNullException(nameof(_content));
        
        using (var fileStream = new FileStream(_filepath, FileMode.OpenOrCreate, FileAccess.Write))
        {
            using var reader = new BinaryReader(_content);            
            Span<byte> buffer = stackalloc byte[1024];
            int readed;
            while ((readed = reader.Read(buffer)) > 0)
            {                
                if (buffer.Length > readed)
                    fileStream.Write(buffer[..readed]);
                else
                    fileStream.Write(buffer);
            }
        }        
    }
}
