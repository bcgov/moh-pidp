import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SetDashboardTitleGuard } from '@pidp/presentation';

import { DriverFitnessPage } from './driver-fitness.page';
import { DriverFitnessResolver } from './driver-fitness.resolver';

const routes: Routes = [
  {
    path: '',
    component: DriverFitnessPage,
    canActivate: [SetDashboardTitleGuard],
    resolve: {
      driverFitnessStatusCode: DriverFitnessResolver,
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
