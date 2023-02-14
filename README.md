# TgToGitlabBot

## Что умеет данный бот

Данный бот умеет получать сообщение с приклепленным файлом из чата/группы/канала и коммитить его в Gitlab (Возможно в любой другой Git).
Бот умеет работать сразу с несколькими чатами/группами/каналами
### Действия, воспринимаемые ботом:

- /status - отображает наличие конфигурации данного чата
- Любое сообщение с приклепленным файлом - добавляет файл в репозиторий, соответствующий данному чату, и делает коммит с сообщением, приклепленным к файлу

## Деплой бота через docker-compose

### Настройка .env docker-compose

Рядом с файлом docker-compose.yml необходимо создать файл .env (.env.example как пример) и добавить в него требуемые переменные окружения:
>TELEGRAM_API_ID={id}
>
>TELEGRAM_API_HASH={hash}
>
>TELEGRAM_PROXY={http://{host}:{port}} - необязателен. Требуется только в том случае, если перед Local Telegram Bot Api есть прокси
>
Получить Api id и Api hash можно следуя инструкции от Telegram: https://core.telegram.org/api/obtaining_api_id

### Настройка Telegram бота

Получить токен бота можно следуя инструкции от Telegram: https://core.telegram.org/bots/features#botfather

Для работы бота необходимо выключить privacy mode. Для этого необходимо:
- В @BotFather использовать команду /mybots
- Перейти в меню Group privacy
- Выключить privacy mode

### Настройка appsettings.json

Файл appsettings.json должен распологаться по пути conf/appsettings.json.
В директории conf распологается файл appsettings.example.json, являющийся шаблоном 

appsettings.example.json
```json
{
  "TelegramBot": {
    "BotToken": "BotToken",
    "BaseUrl": "http://telegram-local-api:8081",    
    "WebhookUrl": "http://botapp:8080/update"
  },
  "GitOptions": {
    "Username": "Bot user username",
    "Email": "Bot user email",
    "ChatOptions": {
      "00000000001": {
        "Url": "repository url",        
        "AccessToken": "access token or password of bot user account",
        "Branch": "master",
        "LocalPath": "repository/repository1",
        "FilePath": null
      },
      "00000000002": {
        "Url": "repository url",        
        "AccessToken": "access token or password of bot user account",
        "Branch": "master",
        "LocalPath": "repository/repository2",
        "FilePath": null
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
### Параметры appsettings.json

> TelegramBot - секция конфигурации Telegram бота
>
> BotToken - токен бота Telegram
> 
> BaseUrl - url локального Telegram Bot Api, в формате: http://{имя контейнера с локальным API}:{порт(8081 по умолчанию)}
>
> WebhookUrl - url вебхука бота, в формате: http://{имя контейнера бота}:{порт(8080 по умолчанию)}/{endpoint(update по умолчанию)}
>
> GitOptions - секция конфигурации git
>
> Username - имя пользователя Git
>
> Email - email пользователя Git
>
>> ChatOptions - секция конфигурации для конкретного чата
>>
>>> ChatId - id чата для которого применяются данная конфигурация. Для получения chatId можно использовать стороннего бота или воспользоваться командой /status у данного бота
>>>
>>> Url - url репозитория
>>>
>>> AccessToken - персональный/проектный/групповой токен доступа или пароль аккаунта Gitlab/другого Git
>>>
>>> Branch - ветка, в которую будут происходить коммиты(ветка должна существовать)
>>>
>>> LocalPath - относительный путь до директории с репозиторием(в случае отсутствия данной директории она будет создана). Важно, чтобы путь до репозитория начинался с **repository**
>>>
>>> FilePath - дополнительный путь для файла, необязателен


Для старта бота введите в командную строку:
>
> cd {путь до корня проекта}
> 
> docker-compose up --detach

## Структура docker-compose

Имеются 2 контейнера:
- botapp - контейнер самого бота
- telegram-local-api - локальный сервер Telegram Bot Api. Оригинальный образ контейнера: https://hub.docker.com/r/aiogram/telegram-bot-api

Имеются следующие тома:
- telegram-bot-api-data - хранилище принимаемых файлов Telegram Bot Api. Монтируется на /var/lib/telegram-bot-api
- telegram-bot-git-data - хранилище репозиториев, с которыми работает бот. Монтируется на /app/repository

## Структура бота

Бот разделен на проекты:
- Bot.App - Основа для запуска бота. Использует ASP .NET Core для работы с Webhook. Имеется класс Program, в котором регистрируются все зависимости и UpdateController для приема входящий через Webhook.
- Bot.ConsoleApp - больше не используется
- Bot.Core - проект с базовыми типами и интерфейсами.
- Bot.Integration.Git - проект, содержащий логику работы с Git. Работает на базе библиотеки LibGit2Sharp. Проект содержит:
  - GitOptions - класс, на который происходит маппинг конфигурации Git
  - GitRepository - класс, через который происходит работа с Git
  - Команды - классы, в которых находится конкретная логика работы с Git
  - DependencyInjection - вспомогательный класс для упрощения регистрации зависимостей
  - GitConfigurationChecker - класс для проверки наличия конфигурации Git
- Bot.Integration.Gitlab - проект, содержащий логику работы с Api Gitlab, больше не используется
- Bot.Integration.Telegram - проект, содержащий логику работы с Telegram. Работает на базе библиотеки Telegram.Bot. Проект содержит:
  - TelegramBot - класс для старта бота. Регистрируется в контейнере зависимостей как HostService
  - TelegramBotOptions - класс конфигурации бота
  - TelegramBotClientWrapper - обертка над ITelegramBotClient. Предназначена для освобождения памяти HttpClient, находящегося внутри TelegramBotCLient
  - TelegramBotUpdateHandler - обработчик входящих Update. Определяет тип update и вызывает подходящий обработчик
  - Обработчики StatusCommandHandler и MessageWithDocumentHandler - обработчики для обработки входящих сообщений
