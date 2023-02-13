# TgToGitlabBot
### Что умеет данный бот

Данный бот умеет получать сообщение с приклепленным файлом из чата/канала и коммитить его в Gitlab (Возможно в любой другой Git).
#### Действия, воспринимаемые ботом:
- /status - отображает наличие конфигурации данного чата
- Любое сообщение с приклепленным файлом

### Деплой бота
#### Настройка .env docker-compose
Рядом с файлом docker-compose.yml необходимо создать файл .env и добавить в него требуемые переменные окружения:
>TELEGRAM_API_ID={id}
>
>TELEGRAM_API_HASH={hash}
>
Получить Api id и Api hash можно следуя инструкции Telegram: https://core.telegram.org/api/obtaining_api_id
#### Настройка appsettings.json
Файл appsettings.json должен распологаться по пти conf/appsettings.json.
Там же распологается файл appsettings.example.json, являющийся шаблоном 
appsettings.json
```json
{
  "TelegramBot": {
    "BotToken": "BotToken",
    "BaseUrl": "http://telegram-local-api:8081",    
    "WebhookUrl": "http://botapp:80/update"
  },
  "GitOptions": {
    "Username": "Bot user username",
    "Email": "Bot user email",
    "ChatOptions": {
      "chat id": { -- Отдельная настройка репозитория для каждого отдельного чата
        "Url": "repository url",        
        "AccessToken": "access token or password of bot user account",
        "Branch": "master",
        "LocalPath": "repository/repository1",
        "FilePath": null
      },
      "chat id 2": { -- Отдельная настройка репозитория для каждого отдельного чата
        "Url": "repository url",        
        "AccessToken": "access token or password of bot user account",
        "Branch": "master",
        "LocalPath": "repository/repository1",
        "FilePath": null
      },
      --...
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
> TelegramBot - настройка Telegram бота
> BotToken - токен бота Telegram
> 
> BaseUrl - url локального Telegram Bot Api
>
> WebhookUrl - url вебхука бота
>
> GitOptions - настройка git
>
> Username - имя пользователя Git
>
> Email - email пользователя Git
>
>> ChatOptions - настройка конфигурации для конкретного чата
>>
>>> ChatId - id чата для которого применяются данная конфигурация
>>>
>>> Url - url репозитория
>>> AccessToken - токен доступа или пароль аккаунта
>>>
>>> Branch - ветка, в которую будут происходить коммиты(ветка должна существовать)
>>>
>>> LocalPath - относительный путь до директории с репозиторием(в случае отсутствия данной директории она будет создана)
>>>
>>> FilePath - дополнительный путь для файла


### Структура docker-compose
Имеются 2 контейнера:
- botapp - контейнер самого бота
- telegram-local-api - локальный сервер Telegram Bot Api

Имеются следующие тома:

- telegram-bot-api-data - хранилище принимаемых файлов Telegram Bot Api
- telegram-bot-git-data - хранилище репозиториев, с которыми работает бот
### Структура бота

Бот разделен на проекты:
- Bot.App - Основа для запуска бота. Использует ASP .NET Core для работы с Webhook. Имеется класс Program, в котором регистрируются все зависимости и UpdateController для приема входящий сообщений от локального сервера Telegram Bot Api.
- Bot.ConsoleApp - больше не используется
- Bot.Core - проект с базовыми типами и интерфейсами.
- Bot.Integration.Git - проект, содержащий логику работы с Git. Работает на базе библиотеки LibGit2Sharp. 
- Bot.Integration.Gitlab - проект, содержащий логику работы с Api Gitlab, больше не используется
- Bot.Integration.Telegram - проект, содержащий логику работы с Telegram. Работает на базе библиотеки Telegram.Bot
