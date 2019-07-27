FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /app
COPY dias.tracker.discord.csproj ./
RUN dotnet restore

COPY . .

RUN dotnet publish -c Release -o out

FROM base AS final
WORKDIR /app
COPY --from=build /app/out .
RUN apt update
RUN apt install -y libopus0 libsodium23 libopus-dev libsodium-dev ffmpeg
COPY ./aki.mp3 .
ENTRYPOINT ["dotnet", "dias.tracker.discord.dll"]
