using Bot.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Bot.Integration.Git;

public static class DependencyInjection
{
    public static IServiceCollection AddGit(this IServiceCollection services)
    {
        services.AddTransient<IGitlabService, GitRepository>();
        services.AddTransient<IConfigurationChecker, GitConfigurationChecker>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GitRepository>());
        return services;
    }
}
