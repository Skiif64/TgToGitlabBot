using Bot.Core.Entities;
using Bot.Core.Options;
using Bot.Integration.Gitlab;
using System.Text;

var options = new GitLabOptions
{
    BranchName = "main",
    ProjectPath = "testgroup6892005%2FTestProject",
    ProjectAccesToken = "glpat-TXzycVYHfca9Xw-oy4C3"
};

var file = new CommitInfo
{
    From = "Skiif",
    FileName = "shit.txt",
    Message = "govno",
    Content = new MemoryStream(Encoding.UTF8.GetBytes("shitshit"))
};

var service = new GitlabService(new GitlabClient(), options, null);
await service.CommitFileAsync(file, default);
