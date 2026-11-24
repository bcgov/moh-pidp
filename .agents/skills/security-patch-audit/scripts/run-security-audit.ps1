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
    "backend/services.plr-intake/Dockerfile",
    "backend/services.endorsement-reminder/Dockerfile",
    "backend/tools.do-work/Dockerfile",
    "workspace/Dockerfile"
)

foreach ($df in $dockerfiles) {
    $fullDfPath = Join-Path $rootDir $df
    if (Test-Path $fullDfPath) {
        $fromLines = Get-Content $fullDfPath | Where-Object { $_ -match "^\s*(ARG|FROM)\s+" }
        $report += "- **$df**:"
        foreach ($line in $fromLines) {
            $report += "  - $($line.Trim())"
        }
    }
}
$report += ""

Write-Host "=== 4. Helm Chart & Deployment NGINX / PostgreSQL Images ===" -ForegroundColor Cyan
$report += "## 4. Helm Chart & Deployment NGINX / PostgreSQL Images`n"

$nginxPropsPath = Join-Path $rootDir "nginx-ver.properties"
$nginxProps = @{}
if (Test-Path $nginxPropsPath) {
    $report += "- **nginx-ver.properties** (single source of truth for NGINX CI/CD):"
    foreach ($line in (Get-Content $nginxPropsPath)) {
        $trimmed = $line.Trim()
        if ($trimmed -and -not $trimmed.StartsWith("#") -and $trimmed -match "^([A-Z0-9_]+)=(.+)$") {
            $nginxProps[$Matches[1]] = $Matches[2].Trim()
            $report += "  - ``$($Matches[1])=$($Matches[2].Trim())``"
        }
    }
}

$pgPropsPath = Join-Path $rootDir "postgres-ver.properties"
$pgProps = @{}
if (Test-Path $pgPropsPath) {
    $report += "- **postgres-ver.properties** (single source of truth for PostgreSQL / Patroni):"
    foreach ($line in (Get-Content $pgPropsPath)) {
        $trimmed = $line.Trim()
        if ($trimmed -and -not $trimmed.StartsWith("#") -and $trimmed -match "^([A-Z0-9_]+)=(.+)$") {
            $pgProps[$Matches[1]] = $Matches[2].Trim()
            $report += "  - ``$($Matches[1])=$($Matches[2].Trim())``"
        }
    }
}

$nginxChartPath = Join-Path $rootDir "charts/nginx/Chart.yaml"
if (Test-Path $nginxChartPath) {
    $appVer = Get-Content $nginxChartPath | Where-Object { $_ -match "^\s*appVersion:" } | Select-Object -First 1
    if ($appVer) {
        $report += "- **charts/nginx/Chart.yaml**: $($appVer.Trim())"
    }
}

$nginxValuesPath = Join-Path $rootDir "charts/nginx/values.yaml"
if (Test-Path $nginxValuesPath) {
    $content = Get-Content $nginxValuesPath -Raw
    $report += "- **charts/nginx/values.yaml** (fallback defaults):"
    if ($content -match "(?ms)^image:\s*\r?\n\s+registry:\s*(\S+)\r?\n\s+repository:\s*(\S+)\r?\n\s+tag:\s*(\S+)") {
        $tagVal = $Matches[3].Trim('"', "'")
        $drift = if ($nginxProps["NGINX_VERSION"] -and $tagVal -ne $nginxProps["NGINX_VERSION"]) { " *(differs from nginx-ver.properties: $($nginxProps['NGINX_VERSION']))*" } else { "" }
        $report += "  - nginx: ``$($Matches[1])/$($Matches[2]):$tagVal``$drift"
    }
    if ($content -match "(?ms)^metrics:.*?^\s+image:\s*\r?\n\s+registry:\s*(\S+)\r?\n\s+repository:\s*(\S+)\r?\n\s+tag:\s*(\S+)") {
        $tagVal = $Matches[3].Trim('"', "'")
        $drift = if ($nginxProps["NGINX_EXPORTER_VERSION"] -and $tagVal -ne $nginxProps["NGINX_EXPORTER_VERSION"]) { " *(differs from nginx-ver.properties: $($nginxProps['NGINX_EXPORTER_VERSION']))*" } else { "" }
        $report += "  - metrics: ``$($Matches[1])/$($Matches[2]):$tagVal``$drift"
    }
    if ($content -match "(?ms)^fluentbit:.*?^\s+imageRegistry:\s*(\S+)") {
        $fbImg = $Matches[1]
        $drift = if ($nginxProps["NGINX_FLUENTBIT_VERSION"] -and -not $fbImg.EndsWith(":$($nginxProps['NGINX_FLUENTBIT_VERSION'])")) { " *(differs from nginx-ver.properties: $($nginxProps['NGINX_FLUENTBIT_VERSION']))*" } else { "" }
        $report += "  - fluentbit: ``$fbImg``$drift"
    }
}

$pgTargets = @(
    @{ Path = "charts/pidp/values.yaml"; Pattern = "(?ms)^image:\s*\r?\n\s+repository:\s*postgresql\r?\n.*?\s+tag:\s*(\S+)"; Label = "pidp helper postgresql"; Key = "POSTGRES_VERSION" },
    @{ Path = "infra/patroni-gold-golddr/helm/patroni-gold-golddr/values-gold.yaml"; Pattern = "(?ms)^image:\s*\r?\n.*?\s+tag:\s*(\S+)"; Label = "patroni gold"; Key = "POSTGRES_VERSION" },
    @{ Path = "infra/patroni-gold-golddr/helm/patroni-gold-golddr/values-golddr.yaml"; Pattern = "(?ms)^image:\s*\r?\n.*?\s+tag:\s*(\S+)"; Label = "patroni golddr"; Key = "POSTGRES_VERSION" },
    @{ Path = "infra/metabase/chart/values.yaml"; Pattern = "(?m)^\s+postgresql_version:\s*(\S+)"; Label = "metabase postgresql"; Key = "POSTGRES_VERSION" }
)
$report += "- **PostgreSQL Helm / Infra Defaults**:"
foreach ($t in $pgTargets) {
    $fullTargetPath = Join-Path $rootDir $t.Path
    if (Test-Path $fullTargetPath) {
        $tContent = Get-Content $fullTargetPath -Raw
        if ($tContent -match $t.Pattern) {
            $tVal = $Matches[1].Trim('"', "'")
            $expected = $pgProps[$t.Key]
            $drift = if ($expected -and $tVal -ne $expected) { " *(differs from postgres-ver.properties: $expected)*" } else { "" }
            $report += "  - $($t.Label) ($($t.Path)): ``$tVal``$drift"
        }
    }
}

$deployValueFiles = Get-ChildItem -Path (Join-Path $rootDir "deploy") -Filter "*_values.yaml" -ErrorAction SilentlyContinue | Sort-Object Name
foreach ($vf in $deployValueFiles) {
    $relPath = $vf.FullName.Substring($rootDir.Length).TrimStart('\', '/').Replace('\', '/')
    $vfLines = Get-Content $vf.FullName
    $inNginx = $false
    $nginxOverrides = @()
    foreach ($line in $vfLines) {
        if ($line -match "^nginx:\s*$") { $inNginx = $true; continue }
        if ($inNginx -and $line -match "^\S") { break }
        if ($inNginx -and $line -match "^\s+(registry|repository|tag|imageRegistry):") {
            $nginxOverrides += $line.Trim()
        }
    }
    if ($nginxOverrides.Count -gt 0) {
        $report += "- **$relPath** (nginx image overrides): $($nginxOverrides -join ', ')"
    } else {
        $report += "- **$relPath**: inherits ``charts/nginx/values.yaml`` defaults"
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
