import { APP_INITIALIZER, NgModule, Provider } from '@angular/core';

import { KeycloakAngularModule } from 'keycloak-angular';

import { KeycloakInitService } from './keycloak-init.service';

export function keycloakFactory(
  keycloakInitService: KeycloakInitService
): () => Promise<void> {
  return (): Promise<void> => keycloakInitService.load();
}

export const keycloakProvider: Provider = {
  provide: APP_INITIALIZER,
  useFactory: keycloakFactory,
  multi: true,
  deps: [KeycloakInitService],
};

@NgModule({
  imports: [KeycloakAngularModule],
  providers: [keycloakProvider],
})
export class KeycloakModule {}
