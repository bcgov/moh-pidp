# Do-Work Tool

This tool is intended as a standalone app to run ad-hoc scripts and migrations using the same Dependancy Injection as the main webapi project.
Add whatever additional services you may need to the DI in `Program.cs`, and update `appsettings.json` with the needed environment variables/screts.
Be sure not to commit any secrets when doing so.
