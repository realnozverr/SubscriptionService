FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY local-packages/ /src/local-packages/
COPY ["SubscriptionService.API/SubscriptionService.API.csproj", "SubscriptionService.API/"]
COPY ["SubscriptionService.Domain/SubscriptionService.Domain.csproj", "SubscriptionService.Domain/"]
COPY ["SubscriptionService.Infrastructure/SubscriptionService.Infrastructure.csproj", "SubscriptionService.Infrastructure/"]
COPY ["SubscriptionService.Application/SubscriptionService.Application.csproj", "SubscriptionService.Application/"]
RUN dotnet nuget add source "/src/local-packages" --name "local-source"
RUN dotnet restore "SubscriptionService.API/SubscriptionService.API.csproj"
COPY . .
WORKDIR "/src/SubscriptionService.API"
RUN dotnet build "./SubscriptionService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SubscriptionService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SubscriptionService.API.dll"]
