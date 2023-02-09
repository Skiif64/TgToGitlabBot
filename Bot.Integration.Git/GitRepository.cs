
using Bot.Core.Entities;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Test.CMD")]
namespace Bot.Integration.Git;

internal class GitRepository
{	
    private readonly GitOptions _options;
    private readonly Identity _identity;
    private CredentialsHandler _credentialsHandler;

    public GitRepository(GitOptions options)
    {
        _options = options;
        _identity = new Identity(_options.Username, _options.Email);
        
        _credentialsHandler = (url, user, type) => new UsernamePasswordCredentials
        {
            Username = _options.Username,
            Password = _options.AccessToken
        };
    }

    public void Commit(string message, CommitInfo info, Stream stream) //TODO: stream into commitInfo
    {
        var signature = new Signature
            (
            _identity,
            DateTimeOffset.UtcNow
            );
        using var repository = new Repository(_options.LocalPath);
        using var fileStream = new FileStream(_options.LocalPath+"/"+info.FileName, FileMode.OpenOrCreate, FileAccess.Write);
        using var writer = new BinaryWriter(fileStream);
        using var reader = new BinaryReader(stream);
        writer.Write(reader.ReadBytes((int)stream.Length));
        Commands.Stage(repository,"*"); //TODO: concrete file status
        var commit = repository.Commit(message, signature, signature);
       
    }

    public void Push()
    {
        using var repository = new Repository(_options.LocalPath);
        var remote = repository.Network.Remotes["origin"];
        
        repository.Network.Push(remote, $@"refs/heads/{_options.Branch}", new PushOptions 
        {
            CredentialsProvider = _credentialsHandler            
        });
    }

    public void Initialize()
    {        
        if (Repository.IsValid(_options.LocalPath))
            return;

        Repository.Clone(_options.Url, _options.LocalPath, new CloneOptions
        {
            BranchName = _options.Branch,
            CredentialsProvider = _credentialsHandler
        });
    }
}
