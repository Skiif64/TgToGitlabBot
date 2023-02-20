using Bot.Integration.Git.GitCommands.Initialize;
using Bot.Integration.Git.GitCommands.PullChanges;
using Bot.Integration.Git.GitCommands.Push;
using LibGit2Sharp;
using MediatR;
using Moq;

namespace Bot.Integration.Git.Tests.Mocks;

internal class CommandHandlersMocks
{
    public static Mock<IRequestHandler<InitializeCommand>> InitializeHandlerMock(string repositoryPath)
    {
        var mock = new Mock<IRequestHandler<InitializeCommand>>();
        mock.Setup(x => x.Handle(It.IsAny<InitializeCommand>(), default))
            .Returns(() =>
            {
                Repository.Init(repositoryPath);
                return Task.CompletedTask;
            });
        return mock;
    }

    public static Mock<IRequestHandler<PullChangesCommand>> PullChangesHandlerMock()
    {
        var mock = new Mock<IRequestHandler<PullChangesCommand>>();
        mock.Setup(x => x.Handle(It.IsAny<PullChangesCommand>(), default))
            .Returns(Task.CompletedTask);
        return mock;
    }

    public static Mock<IRequestHandler<PushCommand>> PushHandlerMock()
    {
        var mock = new Mock<IRequestHandler<PushCommand>>();
        mock.Setup(x => x.Handle(It.IsAny<PushCommand>(), default))
            .Returns(Task.CompletedTask);
        return mock;
    }
}
