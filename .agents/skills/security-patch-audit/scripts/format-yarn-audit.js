// Format Yarn Audit Output into actionable Markdown
const fs = require('node:fs');

const inputFile = process.argv[2];
let rawOutput = '';
try {
  if (inputFile && fs.existsSync(inputFile) && fs.statSync(inputFile).isFile()) {
    rawOutput = fs.readFileSync(inputFile, 'utf8');
  } else {
    rawOutput = fs.readFileSync(0, 'utf8');
  }
} catch (err) {
  console.error('Failed to read yarn audit input:', err.message);
  process.exit(1);
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
  } catch (_ignored) {
    // Non-JSON or informational lines from yarn audit output are intentionally ignored
  }
}

const list = Array.from(advisories.values());
const severityOrder = { critical: 0, high: 1, moderate: 2, low: 3, info: 4 };
list.sort((a, b) => (severityOrder[a.severity] ?? 99) - (severityOrder[b.severity] ?? 99));

const counts = { critical: 0, high: 0, moderate: 0, low: 0, info: 0 };
for (const item of list) {
  if (counts[item.severity] !== undefined) counts[item.severity]++;
}

const md = [
  '### Overview',
  `Found **${list.length}** distinct security advisories.`,
  '',
  '| Severity | Unique Advisories |',
  '| :--- | :--- |',
  `| **Critical** | ${counts.critical} |`,
  `| **High** | ${counts.high} |`,
  `| **Moderate** | ${counts.moderate} |`,
  `| **Low** | ${counts.low} |`,
  ''
];

const criticalAndHigh = list.filter(a => a.severity === 'critical' || a.severity === 'high');
const other = list.filter(a => a.severity !== 'critical' && a.severity !== 'high');

if (criticalAndHigh.length > 0) {
  md.push(
    '### Priority Action Items (Critical & High)',
    '',
    '| Severity | Package | Vulnerable | Patched | Recommendation & Advisory |',
    '| :--- | :--- | :--- | :--- | :--- |'
  );
  for (const item of criticalAndHigh) {
    const sevLabel = item.severity === 'critical' ? '**CRITICAL**' : '**HIGH**';
    const link = item.url ? `[${item.title}](${item.url})` : item.title;
    const cleanRec = item.recommendation.replaceAll('|', String.raw`\|`);
    md.push(`| ${sevLabel} | \`${item.module}\` | \`${item.vulnerable_versions}\` | \`${item.patched_versions}\` | ${cleanRec} <br> -> ${link} |`);
  }
  md.push('');
}

if (other.length > 0) {
  md.push(
    '<details>',
    `<summary><b>View Moderate & Low Advisories (${other.length})</b></summary>`,
    '',
    '| Severity | Package | Vulnerable | Patched | Recommendation & Advisory |',
    '| :--- | :--- | :--- | :--- | :--- |'
  );
  for (const item of other) {
    const sevLabel = item.severity === 'moderate' ? 'Moderate' : 'Low';
    const link = item.url ? `[${item.title}](${item.url})` : item.title;
    const cleanRec = item.recommendation.replaceAll('|', String.raw`\|`);
    md.push(`| ${sevLabel} | \`${item.module}\` | \`${item.vulnerable_versions}\` | \`${item.patched_versions}\` | ${cleanRec} <br> -> ${link} |`);
  }
  md.push('</details>', '');
}

const topPackagesToUpgrade = [...new Set(criticalAndHigh.map(a => a.module))].slice(0, 8);
const upgradeCmd = topPackagesToUpgrade.length > 0
  ? `   yarn upgrade ${topPackagesToUpgrade.join(' ')}`
  : '   yarn upgrade <package-name>';

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

md.push(
  '### Suggested Remediation Steps',
  '1. **Direct Package Updates**:',
  '   For direct dependencies in `workspace/package.json`:',
  '   ```bash',
  '   cd workspace',
  upgradeCmd,
  '   ```',
  '',
  '2. **Transitive Dependency Overrides (`resolutions`)**:',
  '   For nested dependencies locked by third-party packages, add override entries in `workspace/package.json`:',
  '   ```json',
  '   "resolutions": {',
  resLines.join('\n'),
  '   }',
  '   ```',
  '   Then update the lockfile:',
  '   ```bash',
  '   cd workspace && yarn install',
  '   ```',
  ''
);

console.log(md.join('\n'));
