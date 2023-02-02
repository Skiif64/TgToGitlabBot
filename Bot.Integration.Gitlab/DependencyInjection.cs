﻿using Bot.Core.Abstractions;
using Bot.Integration.Gitlab.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Bot.Integration.Gitlab;

public static class DependencyInjection
{
    public static IServiceCollection AddGitlab(this IServiceCollection services)
    {
        services.AddScoped<IGitlabService, GitlabService>();
        services.AddScoped<IGitlabClient, GitlabClient>();
        return services;
    }
}