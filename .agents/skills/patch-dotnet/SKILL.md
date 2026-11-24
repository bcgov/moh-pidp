---
name: patch-dotnet
description: >-
  Applies security patches and dependency updates to all .NET projects across the repository, updates package references in .csproj files, verifies compilation, and executes test suites. Use this skill when updating vulnerable NuGet packages in backend services and utilities.
---

# Patch .NET Skill

This skill guides the agent through patching vulnerable or outdated NuGet dependencies across all .NET projects in the repository.

---

## 1. Discovered .NET Projects

All .NET projects are located under [`backend/`](https://github.com/bcgov/moh-pidp/backend):
- Core solution: [`backend/pidp-backend.sln`](https://github.com/bcgov/moh-pidp/backend/pidp-backend.sln)
  - [`backend/webapi/pidp.csproj`](https://github.com/bcgov/moh-pidp/backend/webapi/pidp.csproj)
  - [`backend/webapi.tests/pidp.tests.csproj`](https://github.com/bcgov/moh-pidp/backend/webapi.tests/pidp.tests.csproj)
  - [`backend/services.plr-intake/plr-intake.csproj`](https://github.com/bcgov/moh-pidp/backend/services.plr-intake/plr-intake.csproj)
  - [`backend/services.plr-intake.tests/plr-intake.tests.csproj`](https://github.com/bcgov/moh-pidp/backend/services.plr-intake.tests/plr-intake.tests.csproj)
  - [`backend/services.endorsement-reminder/endorsement-reminder.csproj`](https://github.com/bcgov/moh-pidp/backend/services.endorsement-reminder/endorsement-reminder.csproj)
  - [`backend/services.endorsement-reminder.tests/endorsement-reminder.tests.csproj`](https://github.com/bcgov/moh-pidp/backend/services.endorsement-reminder.tests/endorsement-reminder.tests.csproj)
  - [`backend/tools.do-work/do-work.csproj`](https://github.com/bcgov/moh-pidp/backend/tools.do-work/do-work.csproj)
- Standalone utilities:
  - [`backend/tools.plr-test-data/plr-test-data.csproj`](https://github.com/bcgov/moh-pidp/backend/tools.plr-test-data/plr-test-data.csproj)
  - [`backend/utilities.plr-manual-intake/plr-manual-intake.csproj`](https://github.com/bcgov/moh-pidp/backend/utilities.plr-manual-intake/plr-manual-intake.csproj)
  - [`backend/utilities.plr-manual-intake.tests/plr-manual-intake.tests.csproj`](https://github.com/bcgov/moh-pidp/backend/utilities.plr-manual-intake.tests/plr-manual-intake.tests.csproj)

---

## 2. Patching Procedure

### Step 1: Scan for Vulnerabilities
Scan all `.csproj` files to identify specific vulnerable packages:
```powershell
Get-ChildItem -Path backend -Recurse -Filter *.csproj | ForEach-Object {
    Write-Host "Scanning $($_.Name)..." -ForegroundColor Cyan
    dotnet list $_.FullName package --vulnerable --include-transitive
}
```

### Step 2: Determine Patched Versions
For any detected vulnerable package (e.g., `System.Security.Cryptography.Pkcs` or `System.Security.Cryptography.Xml`):
1. Query NuGet for the latest stable non-vulnerable release within the target framework:
   ```bash
   dotnet list <project-path> package --outdated
   ```
2. For direct package references, update the `Version` attribute in `<PackageReference Include="..." Version="..." />`.
3. For transitive packages pulled in by parent libraries:
   Add an explicit direct `<PackageReference>` in the project file pinning the package to the patched version (e.g. `<PackageReference Include="System.Security.Cryptography.Pkcs" Version="10.0.1" />`).

### Step 3: Build & Verification
1. Restore and compile the solution:
   ```bash
   dotnet build backend/pidp-backend.sln
   ```
2. Run all backend test suites:
   ```bash
   dotnet test backend/pidp-backend.sln
   ```
3. Re-scan projects to confirm vulnerabilities are resolved:
   ```bash
   dotnet list backend/webapi/pidp.csproj package --vulnerable --include-transitive
   ```

### Step 4: Ensure Newest .NET 10 SDK & Runtime in Container Deployments
All backend services run on containerized .NET 10 environments:
- [`backend/webapi/Dockerfile`](https://github.com/bcgov/moh-pidp/backend/webapi/Dockerfile)
- [`backend/services.plr-intake/Dockerfile`](https://github.com/bcgov/moh-pidp/backend/services.plr-intake/Dockerfile)
- [`backend/services.endorsement-reminder/Dockerfile`](https://github.com/bcgov/moh-pidp/backend/services.endorsement-reminder/Dockerfile)
- [`backend/tools.do-work/Dockerfile`](https://github.com/bcgov/moh-pidp/backend/tools.do-work/Dockerfile)

1. **Verify Base Image Tags**: Ensure each Dockerfile uses `mcr.microsoft.com/dotnet/sdk:10.0-alpine` and `mcr.microsoft.com/dotnet/aspnet:10.0-alpine`. Microsoft publishes monthly security patches directly to the `10.0-alpine` tag.
2. **Force Fresh Runtime Pulls**: When building containers, always pass `--pull` (e.g., `docker build --pull -t ... .`) to ensure the build machine or CI/CD runner fetches the latest published .NET 10 security point release rather than using a stale local layer.
3. **Verify Installed Runtime Version**: Test the freshly built container to verify the runtime version:
   ```bash
   docker run --rm mcr.microsoft.com/dotnet/aspnet:10.0-alpine dotnet --version
   ```
