import { NgModule } from '@angular/core';

import { SharedModule } from '@shared/shared.module';

import { TermsOfAccessAgreementRoutingModule } from './terms-of-access-agreement-routing.module';
import { TermsOfAccessAgreementComponent } from './terms-of-access-agreement.component';

@NgModule({
  declarations: [TermsOfAccessAgreementComponent],
  imports: [TermsOfAccessAgreementRoutingModule, SharedModule],
})
export class TermsOfAccessAgreementModule {}
