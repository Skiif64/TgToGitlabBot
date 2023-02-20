using Bot.Integration.Git.GitCommands.AddFile;
using Bot.Integration.Git.GitCommands.CacheFile;
using Bot.Integration.Git.GitCommands.Initialize;
using Bot.Integration.Git.GitCommands.PullChanges;
using Bot.Integration.Git.GitCommands.Push;
using Bot.Integration.Git.GitCommands.Rollback;
using Bot.Integration.Git.GitCommands.StageAndCommit;
using LibGit2Sharp;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Runtime.CompilerServices;

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
            Url = Path.Combine(Environment.CurrentDirectory, REMOTE_REPOSITORY_PATH)
        });
        _repository = new GitRepository(_sender, options);
    }

    private IServiceProvider SetupServiceProvider() =>
        new ServiceCollection()
        .AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GitRepository>())
        .BuildServiceProvider();

    [SetUp]
    public void SetupRepositories()
    {
        if(Directory.Exists(REPOSITORY_PATH))
            Directory.Delete(REPOSITORY_PATH, true);
        if(Directory.Exists(REMOTE_REPOSITORY_PATH))
            Directory.Delete(REMOTE_REPOSITORY_PATH, true);

        Directory.CreateDirectory(REPOSITORY_PATH);
        Directory.CreateDirectory(REMOTE_REPOSITORY_PATH);

        Repository.Init(REMOTE_REPOSITORY_PATH);
        
        
        
    }



}
