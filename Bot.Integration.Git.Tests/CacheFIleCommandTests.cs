using Bot.Integration.Git.GitCommands.AddFile;
using Bot.Integration.Git.GitCommands.CacheFile;

namespace Bot.Integration.Git.Tests;

public class CacheFileCommandTests
{
    private const string REPOSITORY_PATH = "cachefile-test-repository";

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
    public async Task WhenCacheFile_ThenHashShouldBeEqual(string filename)
    {
        var originalFilepath = $"Fixtures/{filename}";
        await using var stream = File.OpenRead(originalFilepath);
        var expectedHash = Hasher.GetSHA256HashString(originalFilepath);        
        var request = new CacheFileCommand(originalFilepath);
        var handler = new CacheFileCommandHandler();

        var cachedFilepath = await handler.Handle(request, default);

        var actualhash = Hasher.GetSHA256HashString(cachedFilepath);
        Assert.That(actualhash, Is.EqualTo(expectedHash));
    }
}
