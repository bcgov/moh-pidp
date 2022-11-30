import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { AccessModule } from '../../access.module';
import { DriverFitnessRoutingModule } from './driver-fitness-routing.module';
import { DriverFitnessV2Page } from './driver-fitness-v2.page';
import { DriverFitnessPage } from './driver-fitness.page';

@NgModule({
  declarations: [DriverFitnessPage, DriverFitnessV2Page],
  imports: [DriverFitnessRoutingModule, SharedModule, AccessModule],
})
export class DriverFitnessModule {}
