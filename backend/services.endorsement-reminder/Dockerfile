FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine as build-env
WORKDIR /source

ENV DOTNET_CLI_HOME="/opt"
ENV PATH="$PATH:/opt/.dotnet/tools"

COPY services.endorsement-reminder ./services.endorsement-reminder
COPY ../webapi ./webapi

RUN dotnet publish services.endorsement-reminder/endorsement-reminder.csproj -c Release -o /app -r linux-musl-x64 --self-contained false

#----------------------------------------------------------------#
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
WORKDIR /app

COPY --from=build-env /app .

CMD ["./endorsement-reminder"]
