import { FlatCompat } from '@eslint/eslintrc';
import js from '@eslint/js';
import nx from '@nx/eslint-plugin';
import { dirname } from 'node:path';
import { fileURLToPath } from 'node:url';

import baseConfig from '../../eslint.config.mjs';

const compat = new FlatCompat({
  baseDirectory: dirname(fileURLToPath(import.meta.url)),
  recommendedConfig: js.configs.recommended,
});

export default [
  ...baseConfig,
  ...nx.configs['flat/angular'],
  ...compat
    .config({
      parserOptions: {
        project: ['apps/pidp/tsconfig.*?.json'],
      },
      extends: [],
    })
    .map((config) => ({
      ...config,
      files: ['**/*.ts'],
      rules: {
        ...config.rules,
        '@angular-eslint/directive-selector': [
          'error',
          {
            type: 'attribute',
            prefix: ['pidp', 'app'],
            style: 'camelCase',
          },
        ],
        '@angular-eslint/component-selector': [
          'error',
          {
            type: 'element',
            prefix: ['pidp', 'app'],
            style: 'kebab-case',
          },
        ],
        '@angular-eslint/component-class-suffix': [
          'error',
          {
            suffixes: ['Component', 'Page'],
          },
        ],
        '@typescript-eslint/await-thenable': ['error'],
        '@typescript-eslint/explicit-function-return-type': ['error'],
        '@typescript-eslint/explicit-member-accessibility': ['error'],
        '@typescript-eslint/no-for-in-array': ['error'],
        '@typescript-eslint/no-unused-vars': [
          'error',
          {
            vars: 'all',
            args: 'after-used',
            ignoreRestSiblings: false,
            argsIgnorePattern: '_',
          },
        ],
        '@angular-eslint/prefer-standalone': 'off',
        '@angular-eslint/prefer-inject': 'warn',
      },
    })),
  ...nx.configs['flat/angular-template'],
  {
    files: ['**/*.html'],
    rules: {
      '@angular-eslint/template/label-has-associated-control': 'warn',
      '@angular-eslint/template/elements-content': 'off'
    }
  }
];
