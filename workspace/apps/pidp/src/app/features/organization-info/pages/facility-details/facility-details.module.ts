import { NgModule } from '@angular/core';

import { FacilityDetailsRoutingModule } from './facility-details-routing.module';
import { FacilityDetailsComponent } from './facility-details.component';
import { SharedModule } from '@app/shared/shared.module';


@NgModule({
  declarations: [
    FacilityDetailsComponent
  ],
  imports: [
    FacilityDetailsRoutingModule, SharedModule
  ]
})
export class FacilityDetailsModule { }
