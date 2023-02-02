using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Bot.Integration.Gitlab;
using Bot.Integration.Telegram;
using Microsoft.Extensions.DependencyInjection;
using Bot.Core.Options;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureAppConfiguration((host, cfg) => cfg
.AddJsonFile("appsettings.json", false, true));

builder.ConfigureServices((ctx, services) =>
{
    services.Configure<TelegramBotOptions>("TelegramBot", ctx.Configuration);
    services.Configure<GitLabOptions>("Gitlab", ctx.Configuration);
    services.AddGitlab();
    services.AddTelegramBot(ctx.Configuration);    
});

await builder
    .Build()
    .RunAsync();