# Используем образ .NET SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Копируем все проекты решения, чтобы восстановить зависимости
COPY ["Memo.API/Memo.API.csproj", "Memo.API/"]
COPY ["Memo.Application/Memo.Application.csproj", "Memo.Application/"]
COPY ["Memo.Domain/Memo.Domain.csproj", "Memo.Domain/"]
COPY ["Memo.Infrastructure/Memo.Infrastructure.csproj", "Memo.Infrastructure/"]
RUN dotnet restore "Memo.API/Memo.API.csproj"

# Копируем весь код и собираем приложение
COPY . .
WORKDIR "/src/Memo.API"
RUN dotnet build "Memo.API.csproj" -c Release -o /app/build
RUN dotnet publish "Memo.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Финальный образ
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Memo.API.dll"]