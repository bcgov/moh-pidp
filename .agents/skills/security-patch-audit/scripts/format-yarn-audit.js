// Format Yarn Audit Output into actionable Markdown
const fs = require('fs');
const path = require('path');
const { execSync } = require('child_process');

const workspaceDir = process.argv[2] || path.resolve(__dirname, '../../../../workspace');

let rawOutput = '';
try {
  rawOutput = execSync('yarn audit --json', {
    cwd: workspaceDir,
    maxBuffer: 50 * 1024 * 1024,
    stdio: ['ignore', 'pipe', 'ignore']
  }).toString();
} catch (err) {
  if (err.stdout) {
    rawOutput = err.stdout.toString();
  } else {
    console.error('Failed to run yarn audit:', err.message);
    process.exit(1);
  }
}

const advisories = new Map();
const lines = rawOutput.split('\n').filter(Boolean);

for (const line of lines) {
  try {
    const parsed = JSON.parse(line);
    if (parsed.type === 'auditAdvisory' && parsed.data?.advisory) {
      const adv = parsed.data.advisory;
      const key = adv.id || adv.github_advisory_id || (adv.module_name + adv.title);
      if (!advisories.has(key)) {
        advisories.set(key, {
          id: adv.id || adv.github_advisory_id,
          module: adv.module_name,
          severity: (adv.severity || 'moderate').toLowerCase(),
          title: adv.title || 'Security Vulnerability',
          vulnerable_versions: adv.vulnerable_versions || 'All',
          patched_versions: adv.patched_versions || 'None',
          recommendation: adv.recommendation || 'Upgrade package',
          url: adv.url || '',
          paths: new Set(adv.findings?.flatMap(f => f.paths) || [])
        });
      } else {
        const item = advisories.get(key);
        if (adv.findings) {
          for (const f of adv.findings) {
            for (const p of f.paths) {
              item.paths.add(p);
            }
          }
        }
      }
    }
  } catch (_) {}
}

const list = Array.from(advisories.values());
const severityOrder = { critical: 0, high: 1, moderate: 2, low: 3, info: 4 };
list.sort((a, b) => (severityOrder[a.severity] ?? 99) - (severityOrder[b.severity] ?? 99));

const counts = { critical: 0, high: 0, moderate: 0, low: 0, info: 0 };
for (const item of list) {
  if (counts[item.severity] !== undefined) counts[item.severity]++;
}

let md = [];
md.push('### Overview');
md.push(`Found **${list.length}** distinct security advisories.`);
md.push('');
md.push('| Severity | Unique Advisories |');
md.push('| :--- | :--- |');
md.push(`| **Critical** | ${counts.critical} |`);
md.push(`| **High** | ${counts.high} |`);
md.push(`| **Moderate** | ${counts.moderate} |`);
md.push(`| **Low** | ${counts.low} |`);
md.push('');

const criticalAndHigh = list.filter(a => a.severity === 'critical' || a.severity === 'high');
const other = list.filter(a => a.severity !== 'critical' && a.severity !== 'high');

if (criticalAndHigh.length > 0) {
  md.push('### Priority Action Items (Critical & High)');
  md.push('');
  md.push('| Severity | Package | Vulnerable | Patched | Recommendation & Advisory |');
  md.push('| :--- | :--- | :--- | :--- | :--- |');
  for (const item of criticalAndHigh) {
    const sevLabel = item.severity === 'critical' ? '**CRITICAL**' : '**HIGH**';
    const link = item.url ? `[${item.title}](${item.url})` : item.title;
    const cleanRec = item.recommendation.replace(/\|/g, '\\|');
    md.push(`| ${sevLabel} | \`${item.module}\` | \`${item.vulnerable_versions}\` | \`${item.patched_versions}\` | ${cleanRec} <br> -> ${link} |`);
  }
  md.push('');
}

if (other.length > 0) {
  md.push('<details>');
  md.push(`<summary><b>View Moderate & Low Advisories (${other.length})</b></summary>`);
  md.push('');
  md.push('| Severity | Package | Vulnerable | Patched | Recommendation & Advisory |');
  md.push('| :--- | :--- | :--- | :--- | :--- |');
  for (const item of other) {
    const sevLabel = item.severity === 'moderate' ? 'Moderate' : 'Low';
    const link = item.url ? `[${item.title}](${item.url})` : item.title;
    const cleanRec = item.recommendation.replace(/\|/g, '\\|');
    md.push(`| ${sevLabel} | \`${item.module}\` | \`${item.vulnerable_versions}\` | \`${item.patched_versions}\` | ${cleanRec} <br> -> ${link} |`);
  }
  md.push('</details>');
  md.push('');
}

md.push('### Suggested Remediation Steps');
md.push('1. **Direct Package Updates**:');
md.push('   For direct dependencies in `workspace/package.json`:');
md.push('   ```bash');
md.push('   cd workspace');
const topPackagesToUpgrade = [...new Set(criticalAndHigh.map(a => a.module))].slice(0, 8);
if (topPackagesToUpgrade.length > 0) {
  md.push(`   yarn upgrade ${topPackagesToUpgrade.join(' ')}`);
} else {
  md.push('   yarn upgrade <package-name>');
}
md.push('   ```');
md.push('');
md.push('2. **Transitive Dependency Overrides (`resolutions`)**:');
md.push('   For nested dependencies locked by third-party packages, add override entries in `workspace/package.json`:');
md.push('   ```json');
md.push('   "resolutions": {');

const uniqueResolutions = new Map();
for (const a of criticalAndHigh) {
  if (a.patched_versions && a.patched_versions !== 'None' && a.patched_versions !== '<0.0.0') {
    if (!uniqueResolutions.has(a.module)) {
      uniqueResolutions.set(a.module, a.patched_versions);
    }
  }
}

const entries = Array.from(uniqueResolutions.entries()).slice(0, 10);
const resLines = entries.map(([mod, ver], i) => `     "${mod}": "${ver}"${i < entries.length - 1 ? ',' : ''}`);
md.push(resLines.join('\n'));
md.push('   }');
md.push('   ```');
md.push('   Then update the lockfile:');
md.push('   ```bash');
md.push('   cd workspace && yarn install');
md.push('   ```');
md.push('');

console.log(md.join('\n'));
