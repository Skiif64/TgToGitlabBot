using Bot.Core.Entities;
using Bot.Integration.Git.GitCommands.Initialize;
using Bot.Integration.Git.GitCommands.PullChanges;
using Bot.Integration.Git.GitCommands.Push;
using Bot.Integration.Git.Tests.Mocks;
using LibGit2Sharp;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Bot.Integration.Git.Tests;

public class GitRepositoryTests
{
    private const string REPOSITORY_PATH = "git-test-repository";
    private const string REMOTE_REPOSITORY_PATH = "git-remote-test-repository";
    private readonly GitRepository _repository;    
    private readonly IServiceProvider _serviceProvider;
    private readonly ISender _sender;    

    public GitRepositoryTests()
    {
        _serviceProvider = SetupServiceProvider();
        _sender = new Mediator(_serviceProvider);
        var options = new GitOptions
        {
            Username = "test-user",
            Email = "test@email.com"
        };
        options.ChatOptions.Add("1", new GitOptionsSection
        {
            AccessToken = "token",
            Branch = "master",
            FilePath = null,
            LocalPath = REPOSITORY_PATH,
            Url = "file:///"+Path.Combine(Environment.CurrentDirectory, REMOTE_REPOSITORY_PATH)
        });
        _repository = new GitRepository(_sender, options);
    }

    private IServiceProvider SetupServiceProvider() =>
        new ServiceCollection()
        .AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GitRepository>())
        .AddTransient<IRequestHandler<InitializeCommand>>
        (sp => CommandHandlersMocks.InitializeHandlerMock(REPOSITORY_PATH).Object)
        .AddTransient<IRequestHandler<PullChangesCommand>>
        (sp => CommandHandlersMocks.PullChangesHandlerMock().Object)
        .AddTransient<IRequestHandler<PushCommand>>
        (sp => CommandHandlersMocks.PushHandlerMock().Object)
        .BuildServiceProvider();

    [SetUp]
    public void SetupRepositories()
    {
        if(Directory.Exists(REPOSITORY_PATH))
            Directory.Delete(REPOSITORY_PATH, true);
       
        Directory.CreateDirectory(REPOSITORY_PATH);        
    }

    [TestCase("UTF-8-BIG.txt")]
    [TestCase("UTF-8.txt")]
    [TestCase("WINDOWS-1251.txt")]
    public async Task WhenCommitValidFile_ThenShouldReturnSuccessResult(string filename)
    {
        var filepath = $"Fixtures/{filename}";
        var info = new CommitInfo
        {
            FromChatId = 1,
            FileName = filename,
            Content = File.OpenRead(filepath),
            Message = "Test-commit"
        };

        var result = await _repository.CommitFileAndPushAsync(info, default);

        Assert.IsNotNull(result);
        Assert.IsTrue(result);
    }



}
