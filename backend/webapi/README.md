# Provider Identity Portal (PIdP) API / Rebranded as OneHealthID Service

## Local Development Secrets

[ASP.NET Core User Secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-5.0&tabs=windows)

After acquiring the secrets.json file from the appropriate source, open a terminal in the the webapi folder and run
`type .\secrets.json | dotnet user-secrets set` on Windows or
`cat ./secrets.json | dotnet user-secrets set` on macOS / Linux
to set the secrets for local development.
After running the command, the original secrets.json file is no longer needed and should be deleted to avoid accidentally committing it to the repo.

On Windows, the secrets are automatically stored and retrieved from `%APPDATA%\Microsoft\UserSecrets\<user_secrets_id>\secrets.json`. On macOS / Linux, the location is `~/.microsoft/usersecrets/<user_secrets_id>/secrets.json`.
Running the application in Visual Studio Code should be OS agnostic. The local Docker container for the API, however, mounts the secrets file when brought up using `docker-compose`. Since they are stored at a different location for macOS / Linux, review the following lines in the main `docker-compose.yml`:
```yaml
volumes:
  - ./backend/webapi/:/app
  - pidp-webapi-bin:/app/bin
  - pidp-webapi-obj:/app/obj
  - ${APPDATA}/Microsoft/UserSecrets/5c2dc965-00b4-4531-9ff0-9b37193ead9b:/root/.microsoft/usersecrets/5c2dc965-00b4-4531-9ff0-9b37193ead9b
  # Use the following instead if developing on Mac/Linux:
  # - ${HOME}/.microsoft/usersecrets/5c2dc965-00b4-4531-9ff0-9b37193ead9b:/root/.microsoft/usersecrets/5c2dc965-00b4-4531-9ff0-9b37193ead9b
```

The application's user secrets ID is set in the `pidp.csproj` file.
