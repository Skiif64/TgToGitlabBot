namespace Bot.Integration.Gitlab.Primitives.Base;

internal class UpdateAction : CommitAction
{
    public UpdateAction(string filepath, string? content) : base(filepath, content)
    {
        if (content == null)
            throw new ArgumentNullException(nameof(content));
    }

    public UpdateAction(string filepath, Stream contentStream) : base(filepath, contentStream)
    {
    }

    public override ActionType Action { get; } = ActionType.Update;
}
