import { EnvironmentProviders, importProvidersFrom } from '@angular/core';

import {
  AutoRefreshTokenService,
  UserActivityService,
  provideKeycloak,
  withAutoRefreshToken,
} from 'keycloak-angular';

import { environment } from '../../../environments/environment.prod';
import { provideLookup } from '../lookup/lookup';
import { PermissionsService } from '../permissions/permissions.service';

export const provideKeycloakAngular = (): EnvironmentProviders =>
  provideKeycloak({
    config: environment.keycloakConfig.config,
    initOptions: environment.keycloakConfig.initOptions,
    features: [withAutoRefreshToken({ onInactivityTimeout: 'none' })],
    providers: [
      importProvidersFrom(PermissionsService),
      provideLookup(),
      AutoRefreshTokenService,
      UserActivityService,
    ],
  });
