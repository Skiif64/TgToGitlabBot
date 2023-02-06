using Bot.Integration.Gitlab.Primitives.Base;
using System.Net.Http.Json;

namespace Bot.Integration.Gitlab.Requests;

internal class UpdateRequest : CommitRequest
{
    public UpdateRequest(string message, UpdateAction[] actions, GitLabOptions options) : base(options)
    {
        CommitMessage = message;
        Actions = actions;
    }

    public override HttpContent? ToHttpContent() => JsonContent.Create(this);
}
