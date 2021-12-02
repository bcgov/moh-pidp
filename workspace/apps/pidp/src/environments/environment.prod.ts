import { environmentName } from './environment.model';

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
export const environment = {
  apiEndpoint: 'http://localhost:5000',
  environmentName: 'local' as environmentName,
  production: true,
  // TODO what should the config look like now?
  keycloakConfig: {
    config: {
      url: 'https://dev.oidc.gov.bc.ca/auth',
      realm: 'v4mbqqas',
      clientId: 'prime-application-local',
    },
    // TODO why is this a typing error now?
    // initOptions: {
    //   onLoad: 'check-sso',
    // },
  },
  // TODO provide the defaults that are not configurable
};
