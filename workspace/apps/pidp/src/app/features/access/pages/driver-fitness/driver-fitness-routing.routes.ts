import { Routes } from '@angular/router';

import { DriverFitnessPage } from './driver-fitness.page';
import { driverFitnessResolver } from './driver-fitness.resolver';

export const routes: Routes = [
  {
    path: '',
    component: DriverFitnessPage,
    resolve: {
      driverFitnessStatusCode: driverFitnessResolver,
    },
  },
];
