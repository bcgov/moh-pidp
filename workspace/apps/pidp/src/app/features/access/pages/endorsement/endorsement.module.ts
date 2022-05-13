import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { EndorsementRoutingModule } from './endorsement-routing.module';
import { EndorsementPage } from './endorsement.page';

@NgModule({
  declarations: [EndorsementPage],
  imports: [EndorsementRoutingModule, SharedModule],
})
export class EndorsementModule {}
