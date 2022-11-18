import { NgModule } from '@angular/core';

// import { KeycloakAngularModule } from 'keycloak-angular';
// import { KeycloakModule } from '@app/modules/keycloak/keycloak.module';
import { SharedModule } from '@app/shared/shared.module';

import { AccountLinkingRoutingModule } from './account-linking-routing.module';
import { AccountLinkingHomePage } from './pages/home/home.page';

@NgModule({
  declarations: [AccountLinkingHomePage],
  imports: [
    AccountLinkingRoutingModule,
    SharedModule,
    // KeycloakModule,
    // KeycloakAngularModule,
  ],
})
export class AccountLinkingModule {}
