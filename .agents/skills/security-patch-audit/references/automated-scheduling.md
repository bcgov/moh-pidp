# Automated Weekly Scheduling Guide

This document outlines the execution strategies for automating the `security-patch-audit` skill on a weekly basis.

---

## Method 1: Antigravity Scheduled Agent (In-IDE / Background Agent)

Antigravity provides built-in cron scheduling via the `/schedule` command or the background scheduler tool.

### Trigger via Slash Command
In the chat interface, enter:
```text
/schedule
```
Or instruct the agent:
> "Schedule a weekly cron job every Monday at 9:00 AM (`0 9 * * 1`) as a daemon to run the security-patch-audit skill, scan for vulnerabilities across .NET, Yarn, and container base images, and update the security backlog."

### Behind-the-scenes Configuration
The schedule call configures:
- `CronExpression`: `0 9 * * 1` (Every Monday at 9:00 AM)
- `IsDaemon`: `true` (Standing background task that keeps firing independently)
- `Prompt`: "Run the security-patch-audit skill. Check for newly reported vulnerabilities or security advisories across .NET projects, the frontend Yarn workspace, and Docker base images. Report findings and update the backlog."

---

## Method 2: Scheduled GitHub Actions Workflow (Repository CI/CD)

The repository workflow [`.github/workflows/weekly-security-audit.yaml`](https://github.com/bcgov/moh-pidp/.github/workflows/weekly-security-audit.yaml) is scheduled to run every Tuesday at 12:00 PM UTC (`0 12 * * 2`). It publishes findings directly to GitHub Actions Step Summaries and uploads timestamped audit report artifacts.

### Workflow Specification (`.github/workflows/weekly-security-audit.yaml`)

```yaml
name: Weekly Security Patch Audit

on:
  schedule:
    # Every Tuesday at 12:00 PM UTC
    - cron: '0 12 * * 2'
  workflow_dispatch:

jobs:
  audit:
    name: Security Patch Scan
    runs-on: ubuntu-latest
    steps:
      - name: Checkout Code
        uses: actions/checkout@v4

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '10.0.x'

      - name: Setup Node.js & Yarn
        uses: actions/setup-node@v4
        with:
          node-version: '20'

      - name: Audit .NET Vulnerabilities
        run: |
          dotnet list backend/webapi/pidp.csproj package --vulnerable --include-transitive || true
          dotnet list backend/tools.do-work/do-work.csproj package --vulnerable --include-transitive || true

      - name: Audit Frontend Yarn Vulnerabilities
        working-directory: workspace
        run: |
          yarn audit --summary || true
```
