import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { MfaSetupRoutingModule } from './mfa-setup-routing.module';
import { MfaSetupPage } from './mfa-setup.page';

@NgModule({
  declarations: [MfaSetupPage],
  imports: [SharedModule, MfaSetupRoutingModule],
})
export class MfaSetupModule {}
