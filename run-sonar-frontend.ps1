param (
    [string]$Token = "fd23c687e5764837e0f34bf399b7957c80c3c37e",
    [string]$ProjectKey = "watson2_moh-pidp",
    [string]$Organization = "watson2",
    [string]$HostUrl = "https://sonarcloud.io"
)

$ErrorActionPreference = "Stop"

Write-Host "Starting SonarScanner for Angular Frontend..." -ForegroundColor Cyan

# The frontend code is inside the 'workspace' directory
Set-Location .\workspace

# Run the Sonar scanner via npx
Write-Host "Running: npx sonarqube-scanner"
npx sonarqube-scanner `
    -D"sonar.projectKey=$ProjectKey" `
    -D"sonar.organization=$Organization" `
    -D"sonar.sources=apps,libs" `
    -D"sonar.host.url=$HostUrl" `
    -D"sonar.login=$Token" `
    -D"sonar.exclusions=""**/*.spec.ts,**/node_modules/**,**/dist/**""" `
    -D"sonar.tests=apps,libs" `
    -D"sonar.test.inclusions=""**/*.spec.ts""" `
    -D"sonar.javascript.lcov.reportPaths=""coverage/lcov.info"""

# Go back to the root directory
Set-Location ..

Write-Host "Frontend Sonar scan complete!" -ForegroundColor Green
