using MediatR;

namespace Bot.Integration.Git.GitCommands.Base;

internal interface IGitCommandHandler<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{

}

internal interface IGitCommandHandler<in TRequest> : IRequestHandler<TRequest>
    where TRequest : IRequest
{

}
