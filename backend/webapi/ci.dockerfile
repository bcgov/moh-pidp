FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine as build-env

WORKDIR /app

ENV DOTNET_CLI_HOME="/DOTNET_CLI_HOME"
ENV ASPNETCORE_URLS="http://0.0.0.0:5000"

RUN dotnet tool install --global dotnet-ef --version 6.0.0-rc.2.21480.5
ENV PATH="$PATH:/DOTNET_CLI_HOME/.dotnet/tools"

COPY *.csproj ./
RUN dotnet restore
COPY . ./

RUN dotnet publish "pidp.csproj" -c Release -o out /p:MicrosoftNETPlatformLibrary=Microsoft.NETCore.App

RUN dotnet ef migrations script --idempotent --output /app/out/databaseMigrations.sql


FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine

WORKDIR /app

RUN apk add postgresql-client

COPY --from=build-env /app/out .

EXPOSE 5000

ENTRYPOINT ["dotnet", "pidp.dll"]
