using Bot.Core.Entities;
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
    private readonly GitRepository _repository;    
    private readonly IServiceProvider _serviceProvider;
    private readonly ISender _sender;

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
        .AddTransient<IRequestHandler<InitializeCommand>>(sp => _initializeHandlerMock.Object)
        .AddTransient<IRequestHandler<PullChangesCommand>>(sp => _pullHandlerMock.Object)
        .AddTransient<IRequestHandler<AddFileCommand>>(sp => _addfileHandlerMock.Object)
        .AddTransient<IRequestHandler<CacheFileCommand, string>>(sp => _cacheHandlerMock.Object)
        .AddTransient<IRequestHandler<PushCommand>>(sp => _pushHandlerMock.Object)
        .AddTransient<IRequestHandler<StageAndCommitCommand, Commit>>(sp => _stageHandlerMock.Object)
        .AddTransient<IRequestHandler<RollbackCommand>>(sp => _rollbackHandlerMock.Object)
        .BuildServiceProvider();
          
    [SetUp]
    public void SetupMocks()
    {
        _initializeHandlerMock.Reset();
        _addfileHandlerMock.Reset();
        _cacheHandlerMock.Reset();
        _pullHandlerMock.Reset();
        _pushHandlerMock.Reset();
        _addfileHandlerMock.Reset();
        _rollbackHandlerMock.Reset();

        _initializeHandlerMock.Setup(x => x.Handle(It.IsAny<InitializeCommand>(), default))
            .Returns(() =>
            {
                Repository.Init(REPOSITORY_PATH, true);
                return Task.CompletedTask;
            });
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
        _rollbackHandlerMock.Verify(x => x.Handle(It.IsAny<RollbackCommand>(), default), Times.Never);
    }



}
