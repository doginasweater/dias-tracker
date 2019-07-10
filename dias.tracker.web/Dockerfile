FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["dias.tracker.web/dias.tracker.web.csproj", "dias.tracker.web/"]
RUN dotnet restore "dias.tracker.web/dias.tracker.web.csproj"
COPY . .
WORKDIR "/src/dias.tracker.web"
RUN dotnet build "dias.tracker.web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "dias.tracker.web.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "dias.tracker.web.dll"]