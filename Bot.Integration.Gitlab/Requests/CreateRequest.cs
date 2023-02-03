using Bot.Core.Options;
using Bot.Integration.Gitlab.Primitives;
using System.Net.Http.Json;

namespace Bot.Integration.Gitlab.Requests;

internal class CreateRequest : CommitRequest
{
    public CreateRequest(string message, CreateAction[] actions, GitLabOptions options) : base(options)
    {
        CommitMessage = message;
        Actions = actions;
    }

    public override HttpContent? ToHttpContent() => JsonContent.Create(this);
}
