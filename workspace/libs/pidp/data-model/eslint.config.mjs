import { FlatCompat } from '@eslint/eslintrc';
import js from '@eslint/js';
import nx from '@nx/eslint-plugin';
import { dirname } from 'node:path';
import { fileURLToPath } from 'node:url';

import baseConfig from '../../../eslint.config.mjs';
import { sharedAngularRules } from '../../../eslint.rules.mjs';

const compat = new FlatCompat({
  baseDirectory: dirname(fileURLToPath(import.meta.url)),
  recommendedConfig: js.configs.recommended,
});

export default [
  ...baseConfig,
  ...nx.configs['flat/angular'],
  ...compat
    .config({
      extends: [],
    })
    .map((config) => ({
      ...config,
      files: ['**/*.ts'],
      rules: {
        ...config.rules,
        ...sharedAngularRules('pidp'),
      },
    })),
  ...nx.configs['flat/angular-template'],
];
