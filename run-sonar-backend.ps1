param (
    [string]$Token = "fd23c687e5764837e0f34bf399b7957c80c3c37e",
    [string]$ProjectKey = "watson2_moh-pidp",
    [string]$Organization = "watson2",
    [string]$HostUrl = "https://sonarcloud.io"
)

$ErrorActionPreference = "Stop"

$BackendDir = ".\backend"
$SolutionFile = "$BackendDir\pidp-backend.sln"

Write-Host "Starting SonarScanner for .NET Backend..." -ForegroundColor Cyan

# 1. Begin the scan
Write-Host "Running: dotnet sonarscanner begin"
dotnet sonarscanner begin /k:"$ProjectKey" /o:"$Organization" /d:sonar.login="$Token" /d:sonar.host.url="$HostUrl" /d:sonar.cs.vscoveragexml.reportsPaths="coverage.xml"

# 2. Build the project
Write-Host "Running: dotnet build"
dotnet build $SolutionFile

# 3. Optional: Run tests if you want coverage included in the scan
# Write-Host "Running: dotnet test"
# dotnet test $SolutionFile --no-build --collect:"XPlat Code Coverage"

# 4. End the scan and upload to Sonar
Write-Host "Running: dotnet sonarscanner end"
dotnet sonarscanner end /d:sonar.login="$Token"

Write-Host "Backend Sonar scan complete!" -ForegroundColor Green
