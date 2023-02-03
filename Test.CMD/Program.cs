using Bot.Core.Entities;
using Bot.Core.Options;
using Bot.Integration.Gitlab;
using dotenv.net;
using System.Text;

DotEnv.Load();
var options = new GitLabOptions
{
    BranchName = "main",
    ProjectPath = Environment.GetEnvironmentVariable("Gitlab__ProjectPath")!,
    AccessToken = Environment.GetEnvironmentVariable("Gitlab__AccessToken")!
};

var file = new CommitInfo
{
    From = "Skiif",
    FileName = "shit3.txt",
    Message = "commit3",
    Content = new MemoryStream(Encoding.UTF8.GetBytes("shitshit"))
};

var service = new GitlabService(new GitlabClient(), options, null);
await service.CommitFileAsync(file, default);
