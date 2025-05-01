FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/StoriesService/StoriesService.csproj", "src/StoriesService/"]
COPY ["src/Authentication/Authentication.Shared/Authentication.Shared.csproj", "src/Authentication/Authentication.Shared/"]
COPY ["src/Shared/Shared.Infrastructure/Shared.Infrastructure.csproj", "src/Shared/Shared.Infrastructure/"]
RUN dotnet restore "src/StoriesService/StoriesService.csproj"
COPY . .
WORKDIR "/src/src/StoriesService"
RUN dotnet build "./StoriesService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./StoriesService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StoriesService.dll"]
