FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["Src/Api/Evently.Api/Evently.Api.csproj", "Src/Api/Evently.Api/"]
COPY ["Src/Modules/Events/Evently.Modules.Events.Infrastructure/Evently.Modules.Events.Infrastructure.csproj", "Src/Modules/Events/Evently.Modules.Events.Infrastructure/"]
COPY ["Src/Modules/Events/Evently.Modules.Events.Application/Evently.Modules.Events.Application.csproj", "Src/Modules/Events/Evently.Modules.Events.Application/"]
COPY ["Src/Modules/Events/Evently.Modules.Events.Domain/Evently.Modules.Events.Domain.csproj", "Src/Modules/Events/Evently.Modules.Events.Domain/"]
COPY ["Src/Modules/Events/Evently.Modules.Events.Presentation/Evently.Modules.Events.Presentation.csproj", "Src/Modules/Events/Evently.Modules.Events.Presentation/"]
RUN dotnet restore "./Src/Api/Evently.Api/Evently.Api.csproj"
COPY . .
WORKDIR "/src/Src/Api/Evently.Api"
RUN dotnet build "./Evently.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Evently.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Evently.Api.dll"]