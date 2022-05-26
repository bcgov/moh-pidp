import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { DriverFitnessPage } from './driver-fitness.page';
import { DriverFitnessResolver } from './driver-fitness.resolver';

const routes: Routes = [
  {
    path: '',
    component: DriverFitnessPage,
    resolve: {
      driverFitnessStatusCode: DriverFitnessResolver,
    },
    data: {
      title: 'Provider Identity Portal',
      routes: {
        root: '../../',
      },
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class DriverFitnessRoutingModule {}
