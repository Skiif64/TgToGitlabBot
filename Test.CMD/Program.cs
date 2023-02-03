using Bot.Core.Abstractions;
using Bot.Core.Entities;
using Bot.Integration.Gitlab;
using dotenv.net;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text;

DotEnv.Load();
var options = new GitLabOptions
{
    BranchName = "main",
    ProjectNamespace = Environment.GetEnvironmentVariable("Gitlab__ProjectNamespace")!,
    ProjectName = Environment.GetEnvironmentVariable("Gitlab__ProjectName")!,
    AccessToken = Environment.GetEnvironmentVariable("Gitlab__AccessToken")!
};

var file = new CommitInfo
{
    From = "Skiif",
    FileName = "shit5.txt",
    Message = "commi5",
    Content = new MemoryStream(Encoding.UTF8.GetBytes("shitshitshit"))
};
var loggerMock = new Mock<ILogger<IGitlabService>>();

var service = new GitlabService(new GitlabClient(), options, loggerMock.Object);
await service.CommitFileAsync(file, default);
