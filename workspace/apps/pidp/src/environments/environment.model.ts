import { EnvironmentConfig } from './environment-config.model';

export type environmentName = EnvironmentName;

export enum EnvironmentName {
  PRODUCTION = 'prod',
  TEST = 'test',
  DEVELOP = 'dev',
  LOCAL = 'local',
}

export interface AppEnvironment extends EnvironmentConfig {
  // Only indicates that Angular has been built
  // using --configuration=production
  production: boolean;
  emails: {
    providerIdentitySupport: string;
    specialAuthoritySupport: string;
    hcimWebSupportEmail: string;
  };
  urls: {
    bcscSupport: string;
    bcscMobileSetup: string;
    specialAuthority: string;
  };
}
