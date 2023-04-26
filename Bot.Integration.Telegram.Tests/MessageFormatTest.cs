using Bot.Integration.Telegram.CommitFactories;
using Moq;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.Integration.Telegram.Tests;

public class MessageFormatTests
{
    [SetUp]
    public void Setup()
    {
    }

    [TestCase("AAAA_BBBBB_CCCCCC_v45_2022_66_02.7z", "AAAA_BBBBB_CCCCCC.7z")]
    [TestCase("AAAA_BBBBB_CCCCCC_v45.zip", "AAAA_BBBBB_CCCCCC.zip")]
    public async Task WhenChannelCommitRequestFactory_CreateCommitRequest_ThenFilenameShouldBeFormatted(string filename, string expectedFilename)
    {
        var client = new Mock<ITelegramBotClient>().Object;
        var message = new Message
        {
            Chat = new Chat
            {
                Title = string.Empty
            },
            Document = new Document
            {
                FileName = filename,
            }
        };
        var sutMock = new Mock<ChannelCommitRequestFactory>(new object[] { message, client });
        sutMock.Setup(x => x.DownloadFileAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Stream.Null));
        sutMock.CallBase = true;
        var sut = sutMock.Object;

        var actual = await sut.CreateCommitRequestAsync(default);

        Assert.That(actual, Is.Not.Null);
        Assert.That(actual.FileName, Is.EqualTo(expectedFilename));
    }
}