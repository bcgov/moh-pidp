FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine as build-env
WORKDIR /source

ENV DOTNET_CLI_HOME="/opt"
ENV PATH="$PATH:/opt/.dotnet/tools"

RUN apk add --no-cache krb5-libs

RUN dotnet tool install --global dotnet-ef --version "10.*"

COPY *.csproj .
RUN dotnet restore -r linux-musl-x64 /p:PublishReadyToRun=true

COPY . .
RUN dotnet publish -c Release -o /app -r linux-musl-x64 --self-contained false --no-restore

RUN dotnet ef migrations bundle

#----------------------------------------------------------------#
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT="Production"
ENV ASPNETCORE_URLS="http://+:8080"

RUN apk add --no-cache krb5-libs

COPY --from=build-env /app .
COPY --from=build-env /source/efbundle .

RUN cd /app && mkdir -p logs \
    chmod 755 /app/logs/

EXPOSE 8080

CMD ["./pidp"]
