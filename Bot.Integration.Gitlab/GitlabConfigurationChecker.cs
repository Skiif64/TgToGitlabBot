using Bot.Core.Abstractions;
using Microsoft.Extensions.Options;

namespace Bot.Integration.Gitlab;

internal class GitlabConfigurationChecker : IConfigurationChecker
{
    private readonly GitLabOptions _gitLabOptions;

    public GitlabConfigurationChecker(IOptionsSnapshot<GitLabOptions> gitLabOptions)
    {
        _gitLabOptions = gitLabOptions.Value;
    }

    public bool Exists(long chatId)
    {
        return _gitLabOptions.ChatOptions.TryGetValue(chatId.ToString(), out var chatConfig);
    }
}
