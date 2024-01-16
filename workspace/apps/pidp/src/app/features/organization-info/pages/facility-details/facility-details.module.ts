import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { FacilityDetailsRoutingModule } from './facility-details-routing.module';
import { FacilityDetailsPage } from './facility-details.page';

@NgModule({
    imports: [FacilityDetailsRoutingModule, SharedModule, FacilityDetailsPage],
})
export class FacilityDetailsModule {}
