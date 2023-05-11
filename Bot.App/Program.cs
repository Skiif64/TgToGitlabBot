using Bot.Integration.Git;
using Bot.Integration.Telegram;
using Microsoft.Extensions.Logging.Console;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("conf/appsettings.json", false, true)   
    .AddEnvironmentVariables();
builder.Logging.AddSimpleConsole(cfg =>
{
    cfg.SingleLine = true;
    cfg.IncludeScopes = true;
    cfg.TimestampFormat = "dd.MM.yyyy HH:mm UTC ";
    cfg.ColorBehavior = LoggerColorBehavior.Enabled;
});
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.Configure<GitOptions>(builder.Configuration.GetRequiredSection(GitOptions.Path));
builder.Services.Configure<TelegramBotOptions>(builder.Configuration.GetRequiredSection(TelegramBotOptions.Path));
builder.Services.AddGit();
builder.Services.AddTelegramBot();
var app = builder.Build();

app.MapControllers();

await app.RunAsync();
