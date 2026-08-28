export const sharedAngularRules = (prefix) => ({
  '@angular-eslint/directive-selector': [
    'error',
    {
      type: 'attribute',
      prefix,
      style: 'camelCase',
    },
  ],
  '@angular-eslint/component-selector': [
    'error',
    {
      type: 'element',
      prefix,
      style: 'kebab-case',
    },
  ],
  '@angular-eslint/prefer-inject': 'warn',
});

export const sharedTsRules = {
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
};

export const createSharedLibraryConfig = (baseConfig, nx, compat, projectPath, prefix) => [
  ...baseConfig,
  ...nx.configs['flat/angular'],
  ...compat
    .config({
      extends: [],
      parserOptions: {
        project: [projectPath],
      },
    })
    .map((config) => ({
      ...config,
      files: ['**/*.ts'],
      rules: {
        ...config.rules,
        ...sharedAngularRules(prefix),
        ...sharedTsRules,
        '@angular-eslint/prefer-standalone': 'off',
      },
    })),
  ...nx.configs['flat/angular-template'],
];
