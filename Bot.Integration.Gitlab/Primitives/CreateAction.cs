﻿using Bot.Integration.Gitlab.Primitives.Base;
using System.Text;

namespace Bot.Integration.Gitlab.Primitives;

internal class CreateAction : CommitAction
{
    public override ActionType Action { get; } = ActionType.Create;
    public CreateAction(string filename, string content)
    {
        if (content == null)
            throw new ArgumentNullException(nameof(content));
        FilePath = filename;
        Content = content;
    }

    public CreateAction(string filename, Stream contentStream)
    {
        if (contentStream == null)
            throw new ArgumentNullException(nameof(contentStream));
        FilePath = filename;
        using (var sr = new StreamReader(contentStream))
        {
            contentStream.Position = 0;
            Content = sr.ReadToEnd();
        }
        contentStream.Dispose();
    }
}
