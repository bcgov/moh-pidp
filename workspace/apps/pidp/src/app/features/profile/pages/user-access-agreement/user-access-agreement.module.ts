import { NgModule } from '@angular/core';

import { SharedModule } from '@shared/shared.module';

import { UserAccessAgreementDocumentComponent } from './components/user-access-agreement-document/user-access-agreement-document.component';
import { UserAccessAgreementRoutingModule } from './user-access-agreement-routing.module';
import { UserAccessAgreementPage } from './user-access-agreement.page';

@NgModule({
    imports: [UserAccessAgreementRoutingModule, SharedModule, UserAccessAgreementPage, UserAccessAgreementDocumentComponent],
})
export class UserAccessAgreementModule {}
