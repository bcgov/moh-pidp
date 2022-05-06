import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { FacilityDetailsRoutingModule } from './facility-details-routing.module';
import { FacilityDetailsPage } from './facility-details.page';

@NgModule({
  declarations: [FacilityDetailsPage],
  imports: [FacilityDetailsRoutingModule, SharedModule],
})
export class FacilityDetailsModule {}
