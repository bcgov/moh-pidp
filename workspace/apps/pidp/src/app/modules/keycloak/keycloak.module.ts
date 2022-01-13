import { APP_INITIALIZER, NgModule } from '@angular/core';

import { KeycloakAngularModule } from 'keycloak-angular';

import { KeycloakInitService } from './keycloak-init.service';

function keycloakFactory(
  keycloakInitService: KeycloakInitService
): () => Promise<void> {
  return (): Promise<void> => keycloakInitService.load();
}

@NgModule({
  imports: [KeycloakAngularModule],
  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: keycloakFactory,
      multi: true,
      deps: [KeycloakInitService],
    },
  ],
})
export class KeycloakModule {}
