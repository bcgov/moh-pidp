import { EnvironmentProviders, importProvidersFrom } from '@angular/core';

import {
  AutoRefreshTokenService,
  ProvideKeycloakOptions,
  UserActivityService,
  provideKeycloak,
  withAutoRefreshToken,
} from 'keycloak-angular';

import { provideLookup } from '../lookup/lookup';
import { PermissionsService } from '../permissions/permissions.service';

export const provideKeycloakAngular = (
  keycloakConfig: ProvideKeycloakOptions,
): EnvironmentProviders =>
  provideKeycloak({
    config: keycloakConfig.config,
    initOptions: keycloakConfig.initOptions,
    features: [
      withAutoRefreshToken({
        onInactivityTimeout: 'login',
      }),
    ],
    providers: [
      importProvidersFrom(PermissionsService),
      provideLookup(),
      AutoRefreshTokenService,
      UserActivityService,
    ],
  });
