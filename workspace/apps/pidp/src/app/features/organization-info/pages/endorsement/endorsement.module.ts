import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { EndorsementRoutingModule } from './endorsement-routing.module';
import { EndorsementRequestPage } from './pages/endorsement-request/endorsement-request.page';
import { EndorsementRequestsReceivedPage } from './pages/endorsement-requests-received/endorsement-requests-received.page';

@NgModule({
  declarations: [EndorsementRequestPage, EndorsementRequestsReceivedPage],
  imports: [EndorsementRoutingModule, SharedModule],
})
export class EndorsementModule {}
