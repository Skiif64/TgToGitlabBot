using Bot.Core.Entities;
using Bot.Core.Options;
using Bot.Integration.Gitlab;
using Bot.Integration.Gitlab.Requests;

var options = new GitLabOptions
{
    AuthorEmail = "Skiif64@yandex.ru",
    AuthorUsername = "Skiif64",
    BranchName = "main",
    ProjectPath = "43107473",
    ProjectAccesToken = "glpat-cfUSxsKzjSFk8ZWuu-tx"
};

var request = new CommitRequest(new CommitInfo
{
    FileId = "0",
    Content = "example",
    FileName = "example2.txt",
    From = "Skiif64",
    Message = "Example commit2"
}.MapToRequest(options),
options);

var client = new GitlabClient();
await client.SendAsync(request, default);
Console.ReadLine();
