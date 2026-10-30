---
name: security-patch-audit
description: >-
  Audits the codebase for vulnerable dependencies, security patches, and security advisories across .NET projects, the frontend Yarn workspace, Docker base images, and Helm deployment charts. Use this skill when checking for security vulnerabilities, applying patches, or running scheduled weekly security scans.
---

# Security Patch Audit Skill

This skill guides the agent through identifying, reporting, and applying security patches across the repository ecosystems (.NET, Frontend Angular/Yarn, and Containers/Helm).

---

## 1. Quick Audit Execution

To run an automated baseline scan across all projects:

```powershell
pwsh -File .agents/skills/security-patch-audit/scripts/run-security-audit.ps1
```

The script audits:
1. **.NET Projects**: [`backend/webapi/pidp.csproj`](https://github.com/bcgov/moh-pidp/backend/webapi/pidp.csproj), [`backend/tools.do-work/do-work.csproj`](https://github.com/bcgov/moh-pidp/backend/tools.do-work/do-work.csproj), and [`backend/webapi.tests/pidp.tests.csproj`](https://github.com/bcgov/moh-pidp/backend/webapi.tests/pidp.tests.csproj) using `dotnet list package --vulnerable --include-transitive`.
2. **Frontend Workspace**: [`workspace/package.json`](https://github.com/bcgov/moh-pidp/workspace/package.json) and `yarn.lock` using the detailed advisory parser [`format-yarn-audit.js`](https://github.com/bcgov/moh-pidp/.agents/skills/security-patch-audit/scripts/format-yarn-audit.js), which enumerates Critical/High/Moderate advisories, patched versions, and exact remediation commands.
3. **Container Base Images**: Extracts base images from Dockerfiles to review against active CVEs.

Results are written to `reports/security-patch-audit_<timestamp>.md`.

---

## 2. Analyzing & Triaging Vulnerabilities

### A. .NET NuGet Dependencies
For any vulnerable package identified:
1. Check for patched package versions:
   ```bash
   dotnet list <project-path> package --outdated
   ```
2. For transitive dependencies (e.g. `System.Security.Cryptography.Pkcs`), add or bump an explicit `<PackageReference>` in the project file to the patched version.
3. Verify compilation and tests:
   ```bash
   dotnet build backend/webapi/pidp.csproj
   dotnet test backend/webapi.tests/pidp.tests.csproj
   ```

### B. Frontend Yarn Dependencies
1. Run a detailed audit in [`workspace/`](https://github.com/bcgov/moh-pidp/workspace):
   ```bash
   yarn audit
   ```
2. Upgrade vulnerable packages where a non-breaking patch exists:
   ```bash
   yarn upgrade <package-name>
   ```
3. Run frontend unit tests and linting to ensure no regressions:
   ```bash
   npx nx run-many --target=test --all
   ```

### C. Docker Base Images & Helm Charts
1. Review base image tags in:
   - [`backend/webapi/Dockerfile`](https://github.com/bcgov/moh-pidp/backend/webapi/Dockerfile)
   - [`backend/tools.do-work/Dockerfile`](https://github.com/bcgov/moh-pidp/backend/tools.do-work/Dockerfile)
   - [`workspace/Dockerfile`](https://github.com/bcgov/moh-pidp/workspace/Dockerfile)
2. Ensure Alpine/Debian base versions receive security point updates (e.g. `10.0-alpine` point releases).
3. Review Bitnami chart dependencies in [`infra/rabbitmq/`](https://github.com/bcgov/moh-pidp/infra/rabbitmq/) and [`charts/`](https://github.com/bcgov/moh-pidp/charts/).

---

## 3. Reporting & Backlog Integration

1. When new vulnerabilities are detected, categorize them by severity (`Critical`, `High`, `Moderate`, `Low`).
2. Add actionable remediation items to the project backlog (e.g. `backlog.md`) with:
   - Affected package & current version
   - Advisory URL (GHSA / CVE ID)
   - Patched version available
   - Breaking change assessment

---

## 4. Weekly Automated Execution

For details on scheduling this skill to run automatically on a weekly basis via an Antigravity background agent or GitHub Actions, see [Automated Weekly Scheduling Guide](./references/automated-scheduling.md).
