using Bot.Core.Entities;
using Bot.Core.ResultObject;
using Bot.Integration.Git.GitCommands.AddFile;
using Bot.Integration.Git.GitCommands.CacheFile;
using Bot.Integration.Git.GitCommands.Initialize;
using Bot.Integration.Git.GitCommands.PullChanges;
using Bot.Integration.Git.GitCommands.Push;
using Bot.Integration.Git.GitCommands.Rollback;
using Bot.Integration.Git.GitCommands.StageAndCommit;
using Bot.Integration.Git.Tests.Mocks;
using LibGit2Sharp;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Bot.Integration.Git.Tests;

public class GitRepositoryTests
{
    private const string REPOSITORY_PATH = "git-test-repository";
    private const string REMOTE_REPOSITORY_PATH = "git-remote-test-repository";        
    private readonly IServiceProvider _serviceProvider;
    private readonly ISender _sender;
    private readonly GitOptions _options;

    private readonly Mock<IRequestHandler<InitializeCommand>> _initializeHandlerMock = new();
    private readonly Mock<IRequestHandler<AddFileCommand>> _addfileHandlerMock = new();
    private readonly Mock<IRequestHandler<CacheFileCommand, string>> _cacheHandlerMock = new();
    private readonly Mock<IRequestHandler<PullChangesCommand>> _pullHandlerMock = new();
    private readonly Mock<IRequestHandler<PushCommand>> _pushHandlerMock = new();
    private readonly Mock<IRequestHandler<RollbackCommand>> _rollbackHandlerMock = new();
    private readonly Mock<IRequestHandler<StageAndCommitCommand, Commit>> _stageHandlerMock = new();
    public GitRepositoryTests()
    {
        _serviceProvider = SetupServiceProvider();
        _sender = new Mediator(_serviceProvider);
        _options = new GitOptions
        {
            Username = "test-user",
            Email = "test@email.com"
        };
        _options.ChatOptions.Add("1", new GitOptionsSection
        {
            AccessToken = "token",
            Branch = "master",
            FilePath = null,
            LocalPath = REPOSITORY_PATH,
            Url = "file:///"+Path.Combine(Environment.CurrentDirectory, REMOTE_REPOSITORY_PATH)           
        });        
    }

    private IServiceProvider SetupServiceProvider() =>
        new ServiceCollection()
        .AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GitRepository>())
        .AddTransient<IRequestHandler<InitializeCommand>>(sp => _initializeHandlerMock.Object)
        .AddTransient<IRequestHandler<PullChangesCommand>>(sp => _pullHandlerMock.Object)
        //.AddTransient<IRequestHandler<AddFileCommand>>(sp => _addfileHandlerMock.Object)
        //.AddTransient<IRequestHandler<CacheFileCommand, string>>(sp => _cacheHandlerMock.Object)
        //.AddTransient<IRequestHandler<PushCommand>>(sp => _pushHandlerMock.Object)
        //.AddTransient<IRequestHandler<StageAndCommitCommand, Commit>>(sp => _stageHandlerMock.Object)
        //.AddTransient<IRequestHandler<RollbackCommand>>(sp => _rollbackHandlerMock.Object)
        .BuildServiceProvider();


    [SetUp]
    public void ClearDirectories()
    {
        if(Directory.Exists(REPOSITORY_PATH))
            ForceDeleteDirectory(REPOSITORY_PATH);
        if (Directory.Exists(REMOTE_REPOSITORY_PATH))
            ForceDeleteDirectory(REMOTE_REPOSITORY_PATH);
       
        Directory.CreateDirectory(REPOSITORY_PATH);
        Directory.CreateDirectory(REMOTE_REPOSITORY_PATH);
        var path = Repository.Init(REMOTE_REPOSITORY_PATH, true);
       
        //using var repo = new Repository("Cloned");
        //var signature = new Signature(_options.Username, _options.Email, DateTimeOffset.UtcNow);
        //repo.Commit("Initial", signature, signature, new CommitOptions { AllowEmptyCommit= true });
    }
    [SetUp]
    public void SetupMocks()
    {
        _initializeHandlerMock.Reset();
        _addfileHandlerMock.Reset();
        _cacheHandlerMock.Reset();
        _pullHandlerMock.Reset();
        _pushHandlerMock.Reset();
        _stageHandlerMock.Reset();
        _rollbackHandlerMock.Reset();

        _initializeHandlerMock.Setup(x => x.Handle(It.IsAny<InitializeCommand>(), default))
            .Returns(() =>
            {
                var path = Repository.Init(REMOTE_REPOSITORY_PATH, true);                
                Repository.Clone(path, REPOSITORY_PATH);
                return Task.CompletedTask;
            });
    }

    [TestCase("UTF-8-BIG.txt")]
    [TestCase("UTF-8.txt")]
    [TestCase("WINDOWS-1251.txt")]
    public async Task WhenCommitValidFile_ThenShouldReturnSuccessResult(string filename)
    {
        var filepath = $"Fixtures/{filename}";
        await using var file = File.OpenRead(filepath);
        var info = new CommitInfo
        {
            FromChatId = 1,
            FileName = filename,
            Content = file,
            Message = "Test-commit"
        };
        var repository = new GitRepository(_sender, _options);
        var result = await repository.CommitFileAndPushAsync(info, default);

        Assert.IsNotNull(result);
        Assert.IsTrue(result);        
        _rollbackHandlerMock.Verify(x => x.Handle(It.IsAny<RollbackCommand>(), default), Times.Never);
    }

    private static void ForceDeleteDirectory(string path)
    {
        var directory = new DirectoryInfo(path) { Attributes = FileAttributes.Normal };

        foreach (var info in directory.GetFileSystemInfos("*", SearchOption.AllDirectories))
        {
            info.Attributes = FileAttributes.Normal;
        }

        directory.Delete(true);
    }
}
