import { EnvironmentProviders, Provider, importProvidersFrom, inject, provideAppInitializer } from '@angular/core';

import { KeycloakAngularModule, KeycloakService } from 'keycloak-angular';

import { provideLookup } from '../lookup/lookup';
import { PermissionsService } from '../permissions/permissions.service';
import { KeycloakInitService } from './keycloak-init.service';

function keycloakFactory(
  keycloakInitService: KeycloakInitService,
): () => Promise<void> {
  return (): Promise<void> => keycloakInitService.load();
}

export function provideKeycloak(): (Provider | EnvironmentProviders)[] {
  return [
    importProvidersFrom(
      KeycloakService,
      KeycloakAngularModule,
      PermissionsService,
    ),
    provideLookup(),
    provideAppInitializer(() => {
        const initializerFn = (keycloakFactory)(inject(KeycloakInitService));
        return initializerFn();
      }),
  ];
}
