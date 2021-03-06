# PLR Test Data CLI Tool

This tool is used to seed the PLR microservice's database with test data for local development.<br>
By default, the tool targets a locally hosted database on port 5433 (the default for this project). An alternate connection string can be passed into the command's first (optional) argument.<br>
The data is generated by enumerating all combinations of test cards + college identifiers + PLR "statuses". The current convention is to make two records for each possible college identifier:
- one with "good standing" with a 5 digit license number of 0 + card number, e.g. 00012.
- one with "bad standing" with a 5 digit license number of 9 + card number, e.g. 90012

## Installation
The tool must be packaged locally and installed before first use. This same procedure can also be used to update the tool to a new version.
In this folder, invoke `dotnet build`, `dotnet pack` to create the NuGet package, then `dotnet tool install plr-test-data --add-source .\nupkg\` to install the package.

## Use
The tool can be invoked with `dotnet plr-test-data` in any backend folder. Use `dotnet plr-test-data <Connection String>` to target a database other than the default.

## Updating Versions
When updating the tool, please be sure to increment the version prefix in the `.csproj` to make sure the new code gets packaged and installed.

## Troubleshooting
Note that the package in the `nupkg` folder is not accessed after installation; any modifications to that package file will not be seen when invoking the tool using `dotnet plr-test-data`.
The tool is cached in `%UserProfile%\.nuget\packages\plr-test-data`.
