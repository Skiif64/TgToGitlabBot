using Bot.Integration.Gitlab.Primitives.Base;
using System.Text;

namespace Bot.Integration.Gitlab.Primitives;

internal class CreateAction : CommitAction
{
    public override ActionType Action { get; } = ActionType.Create;
    public CreateAction(string filename, string content, string? encoding) 
        : base(filename, content, encoding)
    {
        if (content == null)
            throw new ArgumentNullException(nameof(content));
        
    }

    public CreateAction(string filename, Stream contentStream, string? encoding) 
        : base(filename, contentStream, encoding)
    {
        
    }
}
