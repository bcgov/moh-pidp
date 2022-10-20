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
    specialAuthorityEformsSupport: string;
    hcimAccountTransferSupport: string;
    hcimEnrolmentSupport: string;
    prescriptionRefillRequestEformsSupport: string;
    driverFitnessSupport: string;
    uciSupport: string;
    msTeamsSupport: string;
    doctorsTechnologyOfficeSupport: string;
    phsaInformationAccessAndPrivacyOffice: string;
  };
  urls: {
    bcscSupport: string;
    bcscMobileSetup: string;
    specialAuthority: string;
    doctorsTechnologyOffice: string;
  };
}
