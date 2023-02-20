using Bot.Integration.Git;
using Bot.Integration.Telegram;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("conf/appsettings.json", false, true)   
    .AddEnvironmentVariables();

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.Configure<GitOptions>(builder.Configuration.GetRequiredSection(GitOptions.Path));
builder.Services.Configure<TelegramBotOptions>(builder.Configuration.GetRequiredSection(TelegramBotOptions.Path));
builder.Services.AddTransient<HttpClient>();
builder.Services.AddGit();
builder.Services.AddTelegramBot();
var app = builder.Build();

app.MapControllers();

await app.RunAsync();
