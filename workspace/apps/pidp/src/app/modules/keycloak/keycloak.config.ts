import { EnvironmentProviders, importProvidersFrom } from '@angular/core';

import {
  AutoRefreshTokenService,
  UserActivityService,
  provideKeycloak,
  withAutoRefreshToken,
} from 'keycloak-angular';

import { provideLookup } from '../lookup/lookup';
import { PermissionsService } from '../permissions/permissions.service';

export const provideKeycloakAngular = (): EnvironmentProviders =>
  provideKeycloak({
    config: {
      url: 'https://common-logon-dev.hlth.gov.bc.ca/auth',
      realm: 'moh_applications',
      clientId: 'PIDP-WEBAPP',
    },
    initOptions: {
      onLoad: 'check-sso',
      silentCheckSsoRedirectUri:
        window.location.origin + '/assets/silent-check-sso.html',
    },
    features: [withAutoRefreshToken({ onInactivityTimeout: 'none' })],
    providers: [
      importProvidersFrom(PermissionsService),
      provideLookup(),
      AutoRefreshTokenService,
      UserActivityService,
    ],
  });
