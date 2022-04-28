import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FacilityDetailsRoutingModule } from './facility-details-routing.module';
import { FacilityDetailsComponent } from './facility-details.component';


@NgModule({
  declarations: [
    FacilityDetailsComponent
  ],
  imports: [
    CommonModule,
    FacilityDetailsRoutingModule
  ]
})
export class FacilityDetailsModule { }
