using Bot.Core.Entities;
using Bot.Core.Options;
using Bot.Integration.Gitlab.Entities;

namespace Bot.Integration.Gitlab;

internal static class IntegrationGitlabMapperExtensions
{
    public static CommitRequestDto MapToRequest(this CommitInfo file, GitLabOptions options)
    {
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
                    Content = file.Content
                }
            }
        };
        return request;
    }
}
