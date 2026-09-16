param(
    [string]$CsvPath = "pharmstaff.csv",
    [string]$ConnectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=postgres"
)

$ErrorActionPreference = 'Stop'

if (-not (Test-Path $CsvPath)) {
    Write-Error "CSV file not found at path: $CsvPath"
    exit
}

# Find Npgsql.dll from the built backend project
$npgsqlDll = Get-ChildItem -Path "$PSScriptRoot" -Filter "Npgsql.dll" -Recurse -ErrorAction SilentlyContinue | Select-Object -First 1

if (-not $npgsqlDll) {
    Write-Error "Could not find Npgsql.dll. Please ensure the backend project has been built."
    exit
}

Write-Host "Loading Npgsql driver from: $($npgsqlDll.FullName)"
Add-Type -Path $npgsqlDll.FullName

$data = Import-Csv $CsvPath

Write-Host "Connecting to Postgres database..."
$conn = New-Object Npgsql.NpgsqlConnection($ConnectionString)
$conn.Open()

Write-Host "Processing CSV entries..."

foreach ($row in $data) {
    $enabled = $row.enabled
    
    # Process only if enabled = true
    if ($enabled -ne 'true' -and $enabled -ne '1' -and $enabled -ne $true -and $enabled -ne 'True') {
        continue
    }
    
    $upn = $row.upn
    $pharmacyName = $row.pharmacy
    
    if ([string]::IsNullOrWhiteSpace($upn) -or [string]::IsNullOrWhiteSpace($pharmacyName)) {
        Write-Host "WARNING: Skipping row with missing upn or pharmacy name." -ForegroundColor Yellow
        continue
    }

    # 1. Lookup Party ID by UPN
    $cmdParty = $conn.CreateCommand()
    $cmdParty.CommandText = 'SELECT "PartyId" FROM "Credential" WHERE "IdpId" = @upn LIMIT 1;'
    $p1 = $cmdParty.CreateParameter()
    $p1.ParameterName = "upn"
    $p1.Value = $upn
    $cmdParty.Parameters.Add($p1) | Out-Null
    
    $partyId = $cmdParty.ExecuteScalar()
    $cmdParty.Dispose()
    
    if (-not $partyId) {
        Write-Host "WARNING: Party not found for UPN $upn" -ForegroundColor Yellow
        continue
    }
    
    # 2. Lookup Pharmacy ID by Name
    $cmdPharm = $conn.CreateCommand()
    $cmdPharm.CommandText = 'SELECT "Id" FROM "Pharmacies" WHERE "Name" = @pharmName LIMIT 1;'
    $p2 = $cmdPharm.CreateParameter()
    $p2.ParameterName = "pharmName"
    $p2.Value = $pharmacyName
    $cmdPharm.Parameters.Add($p2) | Out-Null
    
    $pharmacyId = $cmdPharm.ExecuteScalar()
    $cmdPharm.Dispose()
    
    if (-not $pharmacyId) {
        Write-Host "WARNING: Pharmacy not found for name '$pharmacyName'" -ForegroundColor Yellow
        continue
    }
    
    # 3. Check if role already exists
    $cmdCheck = $conn.CreateCommand()
    $cmdCheck.CommandText = 'SELECT "Id" FROM "PharmacyPartyRoles" WHERE "PartyId" = @partyId AND "PharmacyId" = @pharmacyId;'
    $p3 = $cmdCheck.CreateParameter()
    $p3.ParameterName = "partyId"
    $p3.Value = $partyId
    $cmdCheck.Parameters.Add($p3) | Out-Null
    
    $p4 = $cmdCheck.CreateParameter()
    $p4.ParameterName = "pharmacyId"
    $p4.Value = $pharmacyId
    $cmdCheck.Parameters.Add($p4) | Out-Null
    
    $existing = $cmdCheck.ExecuteScalar()
    $cmdCheck.Dispose()
    
    if ($existing) {
        Write-Host "INFO: UPN $upn is already assigned to '$pharmacyName'."
        continue
    }
    
    # 4. Insert into PharmacyPartyRoles (Role = 4 is EndUser)
    $cmdInsert = $conn.CreateCommand()
    $cmdInsert.CommandText = 'INSERT INTO "PharmacyPartyRoles" ("PartyId", "PharmacyId", "Role") VALUES (@partyId, @pharmacyId, 4);'
    
    $p5 = $cmdInsert.CreateParameter()
    $p5.ParameterName = "partyId"
    $p5.Value = $partyId
    $cmdInsert.Parameters.Add($p5) | Out-Null
    
    $p6 = $cmdInsert.CreateParameter()
    $p6.ParameterName = "pharmacyId"
    $p6.Value = $pharmacyId
    $cmdInsert.Parameters.Add($p6) | Out-Null
    
    $cmdInsert.ExecuteNonQuery() | Out-Null
    $cmdInsert.Dispose()
    
    Write-Host "SUCCESS: Assigned UPN $upn to '$pharmacyName' as EndUser." -ForegroundColor Green
}

$conn.Close()
Write-Host "Done."
