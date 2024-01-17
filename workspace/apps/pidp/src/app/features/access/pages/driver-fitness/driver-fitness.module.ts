import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { AccessModule } from '../../access.module';
import { DriverFitnessRoutingModule } from './driver-fitness-routing.module';
import { DriverFitnessPage } from './driver-fitness.page';

@NgModule({
  imports: [
    DriverFitnessRoutingModule,
    SharedModule,
    AccessModule,
    DriverFitnessPage,
  ],
})
export class DriverFitnessModule {}
