using Bot.Core.Abstractions;
using Microsoft.Extensions.Options;

namespace Bot.Integration.Git;

internal class GitConfigurationChecker : IConfigurationChecker
{
    private readonly GitOptions _options;

    public GitConfigurationChecker(IOptionsSnapshot<GitOptions> options)
    {
        _options = options.Value;
    }

    public bool Exists(long chatId)
    {
        return _options.ChatOptions.TryGetValue(chatId.ToString(), out var chatOptions);
    }
}
