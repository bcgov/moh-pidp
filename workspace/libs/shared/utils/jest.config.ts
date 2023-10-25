/* eslint-disable */
export default {
  displayName: 'shared-utils',
  preset: '../../../jest.preset.js',
  setupFilesAfterEnv: [
    '<rootDir>/src/test-setup.ts',
    '../../../node_modules/@hirez_io/jest-single/dist/jest-single.js',
  ],
  globals: {},
  coverageDirectory: '../../../coverage/libs/shared/utils',
  transform: {
    '^.+\\.(ts|mjs|js|html|svg)$': [
      'jest-preset-angular',
      {
        tsconfig: '<rootDir>/tsconfig.spec.json',
        stringifyContentPathRegex: '\\.(html|svg)$',
      },
    ],
  },
  transformIgnorePatterns: ['node_modules/(?!.*\\.mjs$)'],
  snapshotSerializers: [
    'jest-preset-angular/build/serializers/no-ng-attributes',
    'jest-preset-angular/build/serializers/ng-snapshot',
    'jest-preset-angular/build/serializers/html-comment',
  ],
};
