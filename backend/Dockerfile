FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine
WORKDIR /app

# Copy all csproj and restore as distinct layers
COPY ./pidp-backend.sln .
COPY ./webapi/pidp.csproj webapi/pidp.csproj
COPY ./webapi.tests/pidp.tests.csproj webapi.tests/pidp.tests.csproj
COPY ./services.plr-intake/plr-intake.csproj services.plr-intake/plr-intake.csproj
COPY ./services.plr-intake.tests/plr-intake.tests.csproj services.plr-intake.tests/plr-intake.tests.csproj
RUN dotnet restore -r linux-musl-x64 /p:PublishReadyToRun=true

# run tests on docker build
COPY . .
RUN dotnet publish -c Release -o /app -r linux-musl-x64 --self-contained false --no-restore

# run tests on docker build
RUN dotnet test ./pidp-backend.sln

# run tests on docker run
ENTRYPOINT ["dotnet", "test"]