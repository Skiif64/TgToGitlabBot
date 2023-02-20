using Bot.Integration.Git.GitCommands.AddFile;
using Bot.Integration.Git.Tests;
using System.Text;

namespace Bot.Git.Tests;

public class AddFileCommandTests
{
    private const string REPOSITORY_PATH = "addfile-test-repository";
    
    public AddFileCommandTests()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);                
    }

    [SetUp]
    public void ClearDirectory()
    {
        if (Directory.Exists(REPOSITORY_PATH))
            Directory.Delete(REPOSITORY_PATH, true);
        Directory.CreateDirectory(REPOSITORY_PATH);
    }

    [TestCase("UTF-8-BIG.txt")]
    [TestCase("UTF-8.txt")]
    [TestCase("WINDOWS-1251.txt")]
    public async Task WhenAddFile_ThenSHA256HashShouldBeEquals(string filename)
    {
        var originalFilepath = $"Fixtures/{filename}";
        await using var stream = File.OpenRead(originalFilepath);
        var expectedHash = Hasher.GetHashString(originalFilepath);
        var repositoryFilepath = Path.Combine(REPOSITORY_PATH, filename);
        var request = new AddFileCommand(stream, repositoryFilepath);
        var handler = new AddFileCommandHandler();

        await handler.Handle(request, default);

        var actualhash = Hasher.GetHashString(repositoryFilepath);      
        Assert.That(actualhash, Is.EqualTo(expectedHash));

    }    
}
