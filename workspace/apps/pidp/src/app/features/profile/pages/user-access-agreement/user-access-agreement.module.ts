import { NgModule } from '@angular/core';

import { SharedModule } from '@shared/shared.module';

import { UserAccessAgreementRoutingModule } from './user-access-agreement-routing.module';
import { UserAccessAgreementPage } from './user-access-agreement.page';

@NgModule({
  declarations: [UserAccessAgreementPage],
  imports: [UserAccessAgreementRoutingModule, SharedModule],
})
export class UserAccessAgreementModule {}
