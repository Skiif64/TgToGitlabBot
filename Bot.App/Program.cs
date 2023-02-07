using Bot.Integration.Gitlab;
using Bot.Integration.Telegram;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory() + "/conf");
// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.Configure<GitLabOptions>(builder.Configuration.GetRequiredSection(GitLabOptions.Path));
builder.Services.Configure<TelegramBotOptions>(builder.Configuration.GetRequiredSection(TelegramBotOptions.Path));
builder.Services.AddTransient<HttpClient>();
builder.Services.AddGitlab();
builder.Services.AddTelegramBot();
var app = builder.Build();
var sp = app.Services;

if (sp.GetRequiredService<IOptions<TelegramBotOptions>>().Value.UseWebhook)
    app.MapControllers();

await app.RunAsync();
