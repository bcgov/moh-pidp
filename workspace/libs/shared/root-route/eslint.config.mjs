import { FlatCompat } from '@eslint/eslintrc';
import js from '@eslint/js';
import nx from '@nx/eslint-plugin';
import { dirname } from 'node:path';
import { fileURLToPath } from 'node:url';

import baseConfig from '../../../eslint.config.mjs';
import { createSharedLibraryConfig } from '../../../eslint.rules.mjs';

const compat = new FlatCompat({
  baseDirectory: dirname(fileURLToPath(import.meta.url)),
  recommendedConfig: js.configs.recommended,
});

export default createSharedLibraryConfig(baseConfig, nx, compat, 'libs/shared/root-route/tsconfig.*?.json', 'ui');
