using MediatR;

namespace Bot.Integration.Git.GitCommands.Base;

internal interface IGitCommandHandler<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IGitCommand<TResponse>
{

}

internal interface IGitCommandHandler<in TRequest> : IRequestHandler<TRequest>
    where TRequest : IGitCommand
{

}
