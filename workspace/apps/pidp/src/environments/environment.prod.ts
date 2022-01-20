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
  production: true,
  apiEndpoint: 'http://localhost:5000',
  environmentName: EnvironmentName.LOCAL,
  // TODO should be changed to applicationUrl or equivalent
  loginRedirectUrl: 'http://localhost:4200',
  emails: {
    providerIdentitySupport: 'provideridentityportal@gov.bc.ca',
    specialAuthoritySupport: 'satransformation@gov.bc.ca',
  },
  urls: {
    bcscSupport:
      'https://www2.gov.bc.ca/gov/content/governments/government-id/bcservicescardapp/help',
    bcscMobileSetup:
      'https://www2.gov.bc.ca/gov/content?id=034E906146794F84A00535A3F9354913',
    specialAuthority: 'https://www.gov.bc.ca/pharmacarespecialauthority',
  },
  keycloakConfig: {
    config: {
      url: 'https://common-logon-dev.hlth.gov.bc.ca/auth',
      realm: 'moh_applications',
      clientId: 'PIDP-WEBAPP',
    },
    initOptions: {
      onLoad: 'check-sso',
    },
  },
};
