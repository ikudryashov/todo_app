FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/TodoApp.TodoApi/TodoApp.TodoApi.csproj", "src/TodoApp.TodoApi/"]
COPY ["src/TodoApp.Application/TodoApp.Application.csproj", "src/TodoApp.Application/"]
COPY ["src/TodoApp.Domain/TodoApp.Domain.csproj", "src/TodoApp.Domain/"]
COPY ["src/TodoApp.Infrastructure/TodoApp.Infrastructure.csproj", "src/TodoApp.Infrastructure/"]
RUN dotnet restore "src/TodoApp.TodoApi/TodoApp.TodoApi.csproj"
COPY . .
WORKDIR "/src/src/TodoApp.TodoApi"
RUN dotnet build "TodoApp.TodoApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TodoApp.TodoApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TodoApp.TodoApi.dll"]
