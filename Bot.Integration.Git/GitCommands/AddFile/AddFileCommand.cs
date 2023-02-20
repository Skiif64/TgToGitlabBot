using MediatR;

namespace Bot.Integration.Git.GitCommands.AddFile;

internal class AddFileCommand : IRequest
{
    public Stream ContentStream { get; }
    public string Filepath { get; }
    public AddFileCommand(Stream contentStream, string filepath)
    {
        if(ContentStream is null)
            throw new ArgumentNullException(nameof(ContentStream));
        if(string.IsNullOrWhiteSpace(filepath))
            throw new ArgumentNullException(nameof(filepath));

        ContentStream = contentStream;
        Filepath = filepath;
    }
}
