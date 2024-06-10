import { Routes } from '@angular/router';

import { setDashboardTitleGuard } from '@pidp/presentation';

import { DriverFitnessPage } from './driver-fitness.page';
import { driverFitnessResolver } from './driver-fitness.resolver';

export const routes: Routes = [
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
