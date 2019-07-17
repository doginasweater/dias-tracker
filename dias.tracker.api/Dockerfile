FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY dias.tracker.api/dias.tracker.api.csproj dias.tracker.api/
RUN dotnet restore "dias.tracker.api/dias.tracker.api.csproj"
COPY . .
WORKDIR "/src/dias.tracker.api"
RUN dotnet build "dias.tracker.api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "dias.tracker.api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "dias.tracker.api.dll"]
