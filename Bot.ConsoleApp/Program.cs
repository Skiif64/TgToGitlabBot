using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Bot.Integration.Gitlab;
using Bot.Integration.Telegram;
using Microsoft.Extensions.DependencyInjection;
using Bot.Core.Options;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureAppConfiguration((host, cfg) => cfg
.AddJsonFile("appsettings.json", false, true)
.AddEnvironmentVariables());

builder.ConfigureServices((ctx, services) =>
{
    services.Configure<TelegramBotOptions>(ctx.Configuration.GetRequiredSection("TelegramBot"));
    services.Configure<GitLabOptions>(ctx.Configuration.GetRequiredSection("Gitlab"));
    services.AddGitlab();
    services.AddTelegramBot();    
});

await builder
    .Build()
    .RunAsync();