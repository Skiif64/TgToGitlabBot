# Структура бота

## Bot.Core

Bot.Core - центральный проект бота, содержит базовые типы и базовые интерфейсы для бота:
- CommitInfo - предоставляет информацию о коммите (сообщение, id чата, автор, содержимое отправленного файла).
- Result - Result object, который может хранитьб возвращаемое значение или исключение.

### Abstractions

- IConfigurationChecker - интерфейс для сервиса проверки наличия конфигурации.
- IGitlabService - интерфейс для сервиса Git
- ITelegramBot - интерфейс Telegram бота

### Exceptions
  
- ConfigurationNotSetException - исключение незаданной конфигурации
- GitException - исключение Git
- TooLargeException - больше не используется

### Errors

Различные Error необходимы для логгирования причины ошибки в чат.

## Bot.Integration.Telegram
  
Bot.Integration.Telegram - проект, отвечающий за работу с Telegram. Проект содержит:
- TelegramBot - реализация интерфейса ITelegramBot, отвечает за настройку и запуск ITelegramBotClient.
- TelegramBotUpdateHandler - реализация интерфейса IUpdateHandler, отвечает за обработку входящих Update.
- TelegramBotClientWrapper - более не используется.
- TelegramBotOptions - класс конфигурации для бота.
- IHandler\<T\> - интерфейс обработчика сообщения.
- StatusCommandHandler и MessageWithDocumentHandler - реализации IHandler\<Message\>. Содержат конкретную логику для обработки входящих сообщений с командой /status
 и сообщений с приклепленным файлом соответственно.
- DependencyInjection - содержит расширения AddTelegramBot, который региструет реализации в контейнере DI.

### CommitFactories

Более не нужная логика, которая позволяет разделить логику создания CommitInfo, в зависимости от источника сообщений (группа или канал).

ChannelCommitRequestFactory также содержит логику урезания версия и даты на конце имени файла.

### Formatters

Не реализованная логика форматтеров сообщений.

### Utils

Утилитарные классы:

- ChatLogger - логгер, который пишет в группу/канал, указанную в конфигурации.
- FileDownloader - позволяет скачать файл, закрепленный за сообщением.
- FileTypeDetector - более не используется (использовался во время работы с GitLab API)
  
## Bot.Integrations.Git

Данный проект отвечает за взаимодействие с GitLab (не исключено, что возможна работа с любым Git). Основан на паттерне CQRS, реализованном через библиотеку MediatR.

- DependencyInjection - содержит метод расширения AddGit, который регистрирует все зависимости проекта.
- GitConfigurationChecker - проверяет, есть ли конфигурация для конкретного чата.
- GitRepository - главный сервис данного проекта, отвечает за Commit и Push в репозиторий.

### GitCommands

Для работы с GitCommands был использован MediatR. Каждая команда делает одно определенное действие, соответствующие ее названию.

## Bot.App

Bot.App - главный проект бота. Основан на ASP .NET.

- UpdateController - контроллер, который принимает сообщения по Webhook и передает их боту
