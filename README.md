# TgToGitlabBot

### Configuration

appsettings.json
```json
{
  "TelegramBot": {
    "BotToken": "bot token",
    "BaseUrl": "http://telegram-local-api:8081",
    "UseWebhook": true,
    "WebhookUrl": "http://botapp:80/update"
  },
  "Gitlab": {
    "ChatOptions": {  -- для каждого чата, с которым может работать бот, должны быть свои настройки чата
      "chat id": {
        "Project": "namespace/projectName or project id",
        "AccessToken": "group/project/personal token",
        "BranchName": "main",
        "FilePath": "additional file path, should ends with /",
        "AuthorUsername": "",
        "AuthorEmail": ""
      }
    }
    
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```
