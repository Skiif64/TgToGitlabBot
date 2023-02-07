using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Bot.Integration.Gitlab;
using Bot.Integration.Telegram;
using dotenv.net;
using Microsoft.Extensions.DependencyInjection;

var builder = Host.CreateDefaultBuilder(args);

#if DEBUG
DotEnv.Load();
#endif

builder.ConfigureAppConfiguration((host, cfg) => cfg
.AddJsonFile("conf/appsettings.json", false, true)
.AddEnvironmentVariables());

builder.ConfigureServices((ctx, services) =>
{
    services.Configure<GitLabOptions>(ctx.Configuration.GetRequiredSection(GitLabOptions.Path));
    services.Configure<TelegramBotOptions>(ctx.Configuration.GetRequiredSection(TelegramBotOptions.Path));
    services.AddTransient<HttpClient>();
    services.AddGitlab();
    services.AddTelegramBot();
});

await builder
    .Build()
    .RunAsync();