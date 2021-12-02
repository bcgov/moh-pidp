import { AppEnvironment, EnvironmentName } from './environment.model';

/**
 * @description
 * Production environment populated with the default
 * environment information and appropriate overrides.
 *
 * NOTE: This environment is for local development from
 * within a container, and not used within the deployment
 * pipeline. For pipeline config mapping see main.ts and
 * the AppConfigModule.
 */
export const environment: AppEnvironment = {
  apiEndpoint: 'http://localhost:5000',
  environmentName: EnvironmentName.LOCAL,
  production: true,
  keycloakConfig: {
    config: {
      url: 'https://dev.oidc.gov.bc.ca/auth',
      realm: 'v4mbqqas',
      clientId: 'prime-application-local',
    },
    initOptions: {
      onLoad: 'check-sso',
    },
  },
};
