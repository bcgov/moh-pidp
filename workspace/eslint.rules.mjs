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
