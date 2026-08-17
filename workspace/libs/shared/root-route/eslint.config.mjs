import { FlatCompat } from '@eslint/eslintrc';
import js from '@eslint/js';
import nx from '@nx/eslint-plugin';
import { dirname } from 'node:path';
import { fileURLToPath } from 'node:url';

import baseConfig from '../../../eslint.config.mjs';
import { sharedAngularRules, sharedTsRules } from '../../../eslint.rules.mjs';

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
      parserOptions: {
        project: ['libs/shared/root-route/tsconfig.*?.json'],
      },
    })
    .map((config) => ({
      ...config,
      files: ['**/*.ts'],
      rules: {
        ...config.rules,
        ...sharedAngularRules('ui'),
        ...sharedTsRules,
        '@angular-eslint/prefer-standalone': 'off',
      },
    })),
  ...nx.configs['flat/angular-template'],
];
