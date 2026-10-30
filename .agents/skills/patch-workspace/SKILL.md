---
name: patch-workspace
description: >-
  Applies security patches and dependency updates to the frontend Angular and Yarn workspace (workspace/), updates package.json dependencies and resolutions (such as @angular/router), runs yarn install, and validates tests and builds. Use this skill when patching frontend JavaScript/TypeScript dependencies.
---

# Patch Workspace Skill

This skill guides the agent through patching vulnerable direct and transitive dependencies in the frontend workspace ([`workspace/`](https://github.com/bcgov/moh-pidp/workspace)).

---

## 1. Scope & Target Files

- Configuration: [`workspace/package.json`](https://github.com/bcgov/moh-pidp/workspace/package.json)
- Lockfile: [`workspace/yarn.lock`](https://github.com/bcgov/moh-pidp/workspace/yarn.lock)

---

## 2. Patching Procedure

### Step 1: Run Detailed Audit
Review the current vulnerabilities and suggested resolutions:
```powershell
node .agents/skills/security-patch-audit/scripts/format-yarn-audit.js workspace
```

### Step 2: Patch Direct Dependencies
For packages listed under `dependencies` or `devDependencies` in [`workspace/package.json`](https://github.com/bcgov/moh-pidp/workspace/package.json):
1. **Angular Packages (e.g., `@angular/router`)**:
   > [!IMPORTANT]
   > **Keep Angular Package Versions Aligned**: Always keep the entire Angular ecosystem on the same version when applying patches. When bumping `@angular/router` to a new version (e.g. `22.2.4`), simultaneously update all companion Angular dependencies and devDependencies to that exact version (`@angular/core`, `@angular/common`, `@angular/compiler`, `@angular/forms`, `@angular/animations`, `@angular/cdk`, `@angular/material`, `@angular/material-luxon-adapter`, `@angular/platform-browser`, `@angular/platform-browser-dynamic`, `@angular/cli`, `@angular-devkit/build-angular`, `@angular-devkit/core`, `@angular-devkit/schematics`, `@angular/compiler-cli`, `@angular/language-service`, `@schematics/angular`). Never leave mismatched patch versions across the Angular package chain.

   ```json
   "@angular/animations": "22.2.4",
   "@angular/cdk": "22.2.4",
   "@angular/cli": "~22.2.4",
   "@angular/common": "22.2.4",
   "@angular/compiler": "22.2.4",
   "@angular/core": "22.2.4",
   "@angular/forms": "22.2.4",
   "@angular/material": "22.2.4",
   "@angular/material-luxon-adapter": "22.2.4",
   "@angular/platform-browser": "22.2.4",
   "@angular/platform-browser-dynamic": "22.2.4",
   "@angular/router": "22.2.4",
   "@angular-devkit/build-angular": "22.2.4",
   "@angular-devkit/core": "22.2.4",
   "@angular-devkit/schematics": "22.2.4",
   "@angular/compiler-cli": "22.2.4",
   "@angular/language-service": "22.2.4",
   "@schematics/angular": "22.2.4"
   ```
2. For other direct packages, update the version in `workspace/package.json` or run `yarn upgrade <package>`.

### Step 3: Patch Transitive Dependencies via `resolutions`
For nested packages pulled in by parent tools (such as Nx, Webpack, etc.) that have active High/Critical advisories (e.g., `proxy-addr`, `handlebars`, `axios`, `brace-expansion`):
1. Add or update the `"resolutions"` block in [`workspace/package.json`](https://github.com/bcgov/moh-pidp/workspace/package.json):
   ```json
   "resolutions": {
     "proxy-addr": ">=2.0.8",
     "shell-quote": ">=1.11.0",
     "handlebars": ">=4.7.10",
     "piscina": ">=5.3.2",
     "axios": ">=1.20.0"
   }
   ```
2. Run `yarn install` from [`workspace/`](https://github.com/bcgov/moh-pidp/workspace) to resolve and update `yarn.lock`:
   ```bash
   cd workspace
   yarn install
   ```

### Step 4: Verification & Build Validation
1. Verify unit tests:
   ```bash
   cd workspace
   yarn test:ci
   ```
2. Verify production bundle build:
   ```bash
   cd workspace
   yarn build
   ```
   > [!NOTE]
   > When building with `@angular-devkit/build-angular:browser` on newer Angular versions, Webpack performs strict export checking. Ensure that TypeScript interfaces and type-only symbols (such as `SimpleChanges`, `HttpEvent`, `HttpInterceptor`, `Provider`, `DashboardHeaderTheme`, `IDashboard`) are imported using explicit `type` modifiers (e.g., `import { type SimpleChanges } from '@angular/core';`) to avoid runtime export resolution errors.

3. Re-run `yarn audit` to confirm vulnerability resolution:
   ```bash
   cd workspace
   yarn audit --summary
   ```
