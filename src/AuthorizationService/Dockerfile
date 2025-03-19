FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/AuthorizationService/IdentityService.csproj", "src/AuthorizationService/"]
COPY ["src/Authentication/Authentication.Extensions/Authentication.Extensions.csproj", "src/Authentication/Authentication.Extensions/"]
COPY ["src/AwsConfigurators/AwsConfigurators.csproj", "src/AwsConfigurators/"]
COPY ["src/Shared/Shared.Infrastructure/Shared.Infrastructure.csproj", "src/Shared/Shared.Infrastructure/"]
COPY ["src/Shared/Shared.Models/Shared.Models.csproj", "src/Shared/Shared.Models/"]
RUN dotnet restore "src/IdentityService/IdentityService.csproj"
COPY . .
WORKDIR "/src/src/AuthorizationService"
RUN dotnet build "AuthorizationService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AuthorizationService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthorizationService.dll"]
