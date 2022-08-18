import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { AccessModule } from '../../access.module';
import { DriverFitnessRoutingModule } from './driver-fitness-routing.module';
import { DriverFitnessPage } from './driver-fitness.page';

@NgModule({
  declarations: [DriverFitnessPage],
  imports: [DriverFitnessRoutingModule, SharedModule, AccessModule],
})
export class DriverFitnessModule {}
