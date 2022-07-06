import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { EndorsementRoutingModule } from './endorsement-request-routing.module';
import { EndorsementRequestPage } from './endorsement-request.page';

@NgModule({
  declarations: [EndorsementRequestPage],
  imports: [EndorsementRoutingModule, SharedModule],
})
export class EndorsementRequestModule {}
