using Bot.Integration.Gitlab.Primitives.Base;

namespace Bot.Integration.Gitlab.Primitives;

internal class UpdateAction : CommitAction
{
    public UpdateAction(string filepath, string? content, string? encoding)
        : base(filepath, content, encoding)
    {
        if (content == null)
            throw new ArgumentNullException(nameof(content));
    }

    public UpdateAction(string filepath, Stream contentStream, string? encoding)
        : base(filepath, contentStream, encoding)
    {
    }

    public override ActionType Action { get; } = ActionType.Update;
}
