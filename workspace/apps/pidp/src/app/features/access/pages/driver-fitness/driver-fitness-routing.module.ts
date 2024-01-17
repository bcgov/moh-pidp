import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { setDashboardTitleGuard } from '@pidp/presentation';

import { DriverFitnessPage } from './driver-fitness.page';
import { driverFitnessResolver } from './driver-fitness.resolver';

const routes: Routes = [
  {
    path: '',
    component: DriverFitnessPage,
    canActivate: [setDashboardTitleGuard],
    resolve: {
      driverFitnessStatusCode: driverFitnessResolver,
    },
    data: {
      setDashboardTitleGuard: {
        titleText: 'Driver Fitness Practitioner Portal Application',
        titleDescriptionText: '',
      },
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class DriverFitnessRoutingModule {}
