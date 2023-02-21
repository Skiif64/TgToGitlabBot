using MediatR;

namespace Bot.Integration.Git.GitCommands.Base;

internal interface IGitCommand : IRequest
{
}

internal interface IGitCommand<TResponse> : IRequest<TResponse>
{

}
