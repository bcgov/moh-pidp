import { Routes } from '@angular/router';

import { OrganizationInfoRoutes } from './organization-info.routes';

export const routes: Routes = [
  {
    path: OrganizationInfoRoutes.ENDORSEMENTS,
    loadChildren: (): Promise<Routes> =>
      import('./pages/endorsements/endorsements-routing.routes').then(
        (m) => m.routes,
      ),
  },
];
