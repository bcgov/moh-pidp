import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { PidpDataModelModule } from '@pidp/data-model';
import {
  PidpPresentationModule,
  SetDashboardTitleGuard,
} from '@pidp/presentation';

import { DriverFitnessV2Page } from './driver-fitness-v2.page';
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
  {
    path: 'v2',
    component: DriverFitnessV2Page,
    canActivate: [SetDashboardTitleGuard],
    resolve: {
      driverFitnessStatusCode: DriverFitnessResolver,
    },
    data: {
      title: 'Provider Identity Portal',
      routes: {
        root: '../../',
      },
      setDashboardTitleGuard: {
        titleText: 'Driver Fitness Practitioner Portal Application',
        titleDescriptionText: '',
      },
    },
  },
];

@NgModule({
  imports: [
    RouterModule.forChild(routes),
    PidpPresentationModule,
    PidpDataModelModule,
  ],
  exports: [RouterModule],
})
export class DriverFitnessRoutingModule {}
