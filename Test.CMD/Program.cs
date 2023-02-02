using Bot.Core.Entities;
using Bot.Core.Options;
using Bot.Integration.Gitlab;
using dotenv.net;
using System.Text;

DotEnv.Load();
var options = new GitLabOptions
{
    BranchName = "main",
    ProjectPath = Environment.GetEnvironmentVariable("PROJECT_PATH")!,
    AccessToken = Environment.GetEnvironmentVariable("ACCESS_TOKEN")!
};

var file = new CommitInfo
{
    From = "Skiif",
    FileName = "shit2.txt",
    Message = "govno2",
    Content = new MemoryStream(Encoding.UTF8.GetBytes("shitshit"))
};

var service = new GitlabService(new GitlabClient(), options, null);
await service.CommitFileAsync(file, default);
