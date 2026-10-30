# Run Security Patch Audit for moh-pidp
param (
    [string]$OutputPath = ""
)

$ErrorActionPreference = "Continue"
$rootDir = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..\..")).Path

$report = @()
$report += "# Security Patch Audit Report"
$report += "Generated: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')`n"

Write-Host "=== 1. Auditing .NET Backend Dependencies ===" -ForegroundColor Cyan
$report += "## 1. .NET Backend Vulnerabilities`n"

$netProjects = Get-ChildItem -Path (Join-Path $rootDir "backend") -Recurse -Filter *.csproj | 
    Select-Object -ExpandProperty FullName | 
    Sort-Object

$hasNetVulns = $false
foreach ($fullProjPath in $netProjects) {
    $relativeProj = $fullProjPath.Substring($rootDir.Length).TrimStart('\', '/')
    Write-Host "Scanning $relativeProj..." -ForegroundColor Gray
    $output = dotnet list $fullProjPath package --vulnerable --include-transitive 2>&1 | Out-String
    if ($output -match "has the following vulnerable packages") {
        $hasNetVulns = $true
        $report += "### Project: $relativeProj"
        $report += '```text'
        $report += $output.Trim()
        $report += '```'
        Write-Host "Vulnerabilities found in $relativeProj" -ForegroundColor Yellow
    } else {
        $report += "- $relativeProj : No known vulnerable packages found."
    }
}
if (-not $hasNetVulns) {
    Write-Host "No vulnerable .NET packages found." -ForegroundColor Green
}
$report += ""

Write-Host "=== 2. Auditing Frontend Dependencies (workspace/) ===" -ForegroundColor Cyan
$report += "## 2. Frontend (Yarn) Vulnerabilities`n"
$workspacePath = Join-Path $rootDir "workspace"
$formatScript = Join-Path $PSScriptRoot "format-yarn-audit.js"

if (Test-Path $workspacePath) {
    Push-Location $workspacePath
    try {
        if (Test-Path $formatScript) {
            Write-Host "Running detailed yarn audit parser..." -ForegroundColor Gray
            $formattedYarnAudit = yarn audit --json 2>$null | node $formatScript 2>&1 | Out-String
            $report += $formattedYarnAudit.Trim()
        } else {
            Write-Host "Running yarn audit..." -ForegroundColor Gray
            $yarnAudit = yarn audit 2>&1 | Out-String
            $report += '```text'
            $report += $yarnAudit.Trim()
            $report += '```'
        }
    } finally {
        Pop-Location
    }
}
$report += ""

Write-Host "=== 3. Base Image & Container Review ===" -ForegroundColor Cyan
$report += "## 3. Container Base Images`n"
$dockerfiles = @(
    "backend/webapi/Dockerfile",
    "backend/tools.do-work/Dockerfile",
    "workspace/Dockerfile"
)

foreach ($df in $dockerfiles) {
    $fullDfPath = Join-Path $rootDir $df
    if (Test-Path $fullDfPath) {
        $fromLines = Get-Content $fullDfPath | Where-Object { $_ -match "^\s*FROM" }
        $report += "- **$df**:"
        foreach ($line in $fromLines) {
            $report += "  - $($line.Trim())"
        }
    }
}
$report += ""

$reportText = $report -join "`n"

if ([string]::IsNullOrWhiteSpace($OutputPath)) {
    $reportsDir = Join-Path $rootDir "reports"
    if (-not (Test-Path $reportsDir)) {
        New-Item -ItemType Directory -Path $reportsDir | Out-Null
    }
    $timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
    $OutputPath = Join-Path $reportsDir "security-patch-audit_$timestamp.md"
}

$reportText | Out-File -FilePath $OutputPath -Encoding utf8
Write-Host "`nAudit complete! Report written to: $OutputPath" -ForegroundColor Green
