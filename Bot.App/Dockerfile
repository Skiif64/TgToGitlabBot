#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Bot.App/Bot.App.csproj", "Bot.App/"]
RUN dotnet restore "Bot.App/Bot.App.csproj"
COPY . .
WORKDIR "/src/Bot.App"
RUN dotnet build "Bot.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bot.App.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bot.App.dll"]