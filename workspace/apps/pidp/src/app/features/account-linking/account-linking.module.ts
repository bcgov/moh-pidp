import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { AccountLinkingRoutingModule } from './account-linking-routing.module';
import { AccountLinkingHomePage } from './pages/home/home.page';

@NgModule({
  declarations: [AccountLinkingHomePage],
  imports: [AccountLinkingRoutingModule, SharedModule],
})
export class AccountLinkingModule {}
