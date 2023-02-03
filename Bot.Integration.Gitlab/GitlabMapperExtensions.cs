using Bot.Core.Entities;
using Bot.Core.Options;
using Bot.Integration.Gitlab.Entities;

namespace Bot.Integration.Gitlab;

internal static class GitlabMapperExtensions
{
    public static CommitRequestDto MapToRequest(this CommitInfo file, GitLabOptions options)
    {
        var content = string.Empty;
        if (file.Content != null)
        {
            using var sr = new StreamReader(file.Content);
            content = sr.ReadToEnd();
        }
        var request = new CommitRequestDto
        {
            Branch = options.BranchName,
            CommitMessage = file.Message,
            Actions = new[]
            {
                new CommitActionDto
                {
                    Action = "create",
                    FilePath = file.FileName,
                    Content = content
                }
            }
        };
        return request;
    }
}
