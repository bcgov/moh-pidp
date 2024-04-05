import { Routes } from '@angular/router';

import { OrganizationInfoRoutes } from './organization-info.routes';

export const routes: Routes = [
  {
    path: OrganizationInfoRoutes.ORGANIZATION_DETAILS,
    loadChildren: (): Promise<Routes> =>
      import(
        './pages/organization-details/organization-details-routing.routes'
      ).then((m) => m.routes),
  },
  {
    path: OrganizationInfoRoutes.ENDORSEMENTS,
    loadChildren: (): Promise<Routes> =>
      import('./pages/endorsements/endorsements-routing.routes').then(
        (m) => m.routes,
      ),
  },
];
