import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { EndorsementRequestsReceivedRoutingModule } from './endorsement-requests-received-routing.module';
import { EndorsementRequestsReceivedPage } from './endorsement-requests-received.page';

@NgModule({
  declarations: [EndorsementRequestsReceivedPage],
  imports: [EndorsementRequestsReceivedRoutingModule, SharedModule],
})
export class EndorsementRequestsReceivedModule {}
