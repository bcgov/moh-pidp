FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine as build-env
WORKDIR /source

ENV DOTNET_CLI_HOME="/opt"
ENV PATH="$PATH:/opt/.dotnet/tools"

RUN apk add --no-cache krb5-libs

COPY tools.do-work ./tools.do-work
COPY webapi ./webapi

RUN dotnet publish tools.do-work/do-work.csproj -c Release -o /app -r linux-musl-x64 --self-contained false

#----------------------------------------------------------------#
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine
WORKDIR /app

RUN apk add --no-cache krb5-libs

COPY --from=build-env /app .

ENTRYPOINT ["./DoWork"]
