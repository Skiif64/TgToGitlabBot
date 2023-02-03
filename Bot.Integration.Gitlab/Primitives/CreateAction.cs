namespace Bot.Integration.Gitlab.Primitives;

internal class CreateAction : CommitActionDto
{
    public override ActionEnum Action { get; } = ActionEnum.Create;
    public CreateAction(string filename, string content)
    {
        if(content == null)
            throw new ArgumentNullException(nameof(content));
        FilePath = filename;
        Content = content;
    }

    public CreateAction(string filename, Stream contentStream)
    {
        FilePath = filename;
        using var sr = new StreamReader(contentStream);
        Content = sr.ReadToEnd();
    }
}
