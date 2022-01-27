import { NgModule } from '@angular/core';

import { SharedModule } from '@shared/shared.module';

import { UserAccessAgreementRoutingModule } from './user-access-agreement-routing.module';
import { UserAccessAgreementComponent } from './user-access-agreement.component';

@NgModule({
  declarations: [UserAccessAgreementComponent],
  imports: [UserAccessAgreementRoutingModule, SharedModule],
})
export class UserAccessAgreementModule {}
