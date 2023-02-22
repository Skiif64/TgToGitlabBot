using Bot.Core.Entities;
using Bot.Integration.Git.GitCommands.Initialize;
using Bot.Integration.Git.GitCommands.PullChanges;
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
    private readonly Mock<IRequestHandler<PullChangesCommand>> _pullHandlerMock = new();
    
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
        .AddTransient(sp => _initializeHandlerMock.Object)
        .AddTransient(sp => _pullHandlerMock.Object)        
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
    }
    [SetUp]
    public void SetupMocks()
    {
        _initializeHandlerMock.Reset();        
        _pullHandlerMock.Reset();        

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
    [TestCase("binary.zip")]
    public async Task WhenCommitValidFile_ThenShouldReturnSuccessResult(string filename)
    {
        var filepath = $"Fixtures/{filename}";
        await using var file = File.OpenRead(filepath);
        var info = new CommitRequest
        {
            FromChatId = 1,
            FileName = filename,
            Content = file,
            Message = "Test-commit"
        };
        var repository = new GitRepository(_sender, _options);
        var result = await repository.CommitFileAndPushAsync(info, default);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Success, Is.True);                
    }

    [TestCase("UTF-8-BIG.txt")]
    [TestCase("UTF-8.txt")]
    [TestCase("WINDOWS-1251.txt")]
    [TestCase("binary.zip")]
    public async Task WhenOverrideCommit_ThenShouldReturnSuccessResult(string filename)
    {
        var filepath = $"Fixtures/{filename}";
        var overrideFilepath = $"Fixtures/Overrides/{filename}";
        await using var file = File.OpenRead(filepath);
        await using var overrideFile = File.OpenRead(overrideFilepath);
        var info = new CommitRequest
        {
            FromChatId = 1,
            FileName = filename,
            Content = file,
            Message = "Test-commit"
        };
        var overrideInfo = new CommitRequest
        {
            FromChatId = 1,
            FileName = filename,
            Content = overrideFile,
            Message = "Test-override-commit"
        };
        var repository = new GitRepository(_sender, _options);        

        var commitResult = await repository.CommitFileAndPushAsync(info, default);       
        var overrideCommitResult = await repository.CommitFileAndPushAsync(overrideInfo, default);        
       
        Assert.That(commitResult, Is.Not.Null);
        Assert.That(overrideCommitResult, Is.Not.Null);
        Assert.That(commitResult.Success, Is.True);
        Assert.That(overrideCommitResult.Success, Is.True);        
    }

    [TestCase("UTF-8-BIG.txt")]
    [TestCase("UTF-8.txt")]
    [TestCase("WINDOWS-1251.txt")]
    [TestCase("binary.zip")]
    public async Task WhenOverrideCommitFailureOnPush_ThenFileShouldNotChangedReturnErrorResultAndHashHasBeenSame(string filename)
    {
        var filepath = $"Fixtures/{filename}";
        var overrideFilepath = $"Fixtures/Overrides/{filename}";
        await using var file = File.OpenRead(filepath);
        await using var overrideFile = File.OpenRead(overrideFilepath);
        var info = new CommitRequest
        {
            FromChatId = 1,
            FileName = filename,
            Content = file,
            Message = "Test-commit"
        };
        var overrideInfo = new CommitRequest
        {
            FromChatId = 1,
            FileName = filename,
            Content = overrideFile,
            Message = "Test-override-commit"
        };
        var repository = new GitRepository(_sender, _options);
        var expectedHash = Hasher.GetHashString(filepath);

        var commitResult = await repository.CommitFileAndPushAsync(info, default);
        ForceDeleteDirectory(REMOTE_REPOSITORY_PATH); //For throwing exception on push
        var overrideCommitResult = await repository.CommitFileAndPushAsync(overrideInfo, default);
        
        var actualHash = Hasher.GetHashString(Path.Combine(REPOSITORY_PATH, filename));
        Assert.That(commitResult, Is.Not.Null);
        Assert.That(overrideCommitResult, Is.Not.Null);
        Assert.That(commitResult.Success, Is.True);
        Assert.That(overrideCommitResult.Success, Is.False);
        Assert.That(actualHash, Is.EqualTo(expectedHash));
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
