# Структура бота

## Bot.Core

Bot.Core - центральный проект бота, содержит базовые типы и базовые интерфейсы для бота:
- CommitInfo - предоставляет информацию о коммите (сообщение, id чата, автор, содержимое отправленного файла).
- Result - Result object, который может хранитьб возвращаемое значение или исключение.
  
- IConfigurationChecker - интерфейс для сервиса проверки наличия конфигурации.
- IGitlabService - интерфейс для сервиса Git
- ITelegramBot - интерфейс Telegram бота
  
- ConfigurationNotSetException - исключение незаданной конфигурации
- GitException - исключение Git
- TooLargeException - больше не используется
  
## Bot.Integration.Telegram
  
Bot.Integration.Telegram - проект, отвечающий за работу с Telegram. Проект содержит:
- TelegramBot - реализация интерфейса ITelegramBot, отвечает за настройку и запуск ITelegramBotClient.
- TelegramBotUpdateHandler - реализация интерфейса IUpdateHandler, отвечает за обработку входящих Update.
- TelegramBotClientWrapper - обертка над стандартным TelegramBotClient.
- TelegramBotOptions - класс конфигурации для бота.
- IHandler\<T\> - интерфейс обработчика сообщения.
- StatusCommandHandler и MessageWithDocumentHandler - реализации IHandler\<Message\>. Содержат конкретную логику для обработки входящих сообщений с командой /status
 и сообщений с приклепленным файлом соответственно.
- DependencyInjection - класс с методом расширения AddTelegramBot, который региструет реализации в контейнере DI.
  
