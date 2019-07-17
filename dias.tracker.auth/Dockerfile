FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY dias.tracker.auth/dias.tracker.auth.csproj dias.tracker.auth/
RUN dotnet restore "dias.tracker.auth/dias.tracker.auth.csproj"
COPY . .
WORKDIR "/src/dias.tracker.auth"
RUN dotnet build "dias.tracker.auth.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "dias.tracker.auth.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "dias.tracker.auth.dll"]
