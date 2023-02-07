#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Bot.ConsoleApp/Bot.ConsoleApp.csproj", "Bot.ConsoleApp/"]
COPY ["Bot.Core/Bot.Core.csproj", "Bot.Core/"]
COPY ["Bot.Integration.Gitlab/Bot.Integration.Gitlab.csproj", "Bot.Integration.Gitlab/"]
COPY ["Bot.Integration.Telegram/Bot.Integration.Telegram.csproj", "Bot.Integration.Telegram/"]
RUN dotnet restore "Bot.ConsoleApp/Bot.ConsoleApp.csproj"
COPY . .
WORKDIR "/src/Bot.ConsoleApp"
RUN dotnet build "Bot.ConsoleApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bot.ConsoleApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bot.ConsoleApp.dll"]