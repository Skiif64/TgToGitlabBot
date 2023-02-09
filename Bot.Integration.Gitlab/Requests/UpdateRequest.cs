using Bot.Integration.Gitlab.Primitives;
using System.Net.Http.Json;

namespace Bot.Integration.Gitlab.Requests;

internal class UpdateRequest : CommitRequest
{
    public UpdateRequest(string message, UpdateAction[] actions, GitlabChatOptions options) : base(options)
    {
        CommitMessage = message;
        Actions = actions;
    }

    public override HttpContent? ToHttpContent() => JsonContent.Create(this);
}
