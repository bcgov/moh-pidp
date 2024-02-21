import { Routes } from '@angular/router';

import { TrainingRoutes } from './training.routes';

export const routes: Routes = [
  {
    path: TrainingRoutes.COMPLIANCE_TRAINING,
    loadChildren: (): Promise<Routes> =>
      import(
        './pages/compliance-training/compliance-training-routing.routes'
      ).then((m) => m.routes),
  },
];
