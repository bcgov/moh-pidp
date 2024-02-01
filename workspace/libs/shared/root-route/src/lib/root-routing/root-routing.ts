import { Routes } from '@angular/router';

import { AccessDeniedComponent } from './pages/access-denied/access-denied.component';
import { MaintenanceComponent } from './pages/maintenance/maintenance.component';
import { PageNotFoundComponent } from './pages/page-not-found/page-not-found.component';
import { RootRoutes } from './root.routes';

export const rootRoutes: Routes = [
  {
    path: RootRoutes.DENIED,
    component: AccessDeniedComponent,
    data: {
      title: 'Access Denied',
    },
  },
  {
    path: RootRoutes.MAINTENANCE,
    component: MaintenanceComponent,
    data: {
      title: 'Under Scheduled Maintenance',
    },
  },
  {
    // Allows for direct routing to page not found
    path: RootRoutes.PAGE_NOT_FOUND,
    component: PageNotFoundComponent,
    data: {
      title: 'Page Not Found',
    },
  },
  {
    path: RootRoutes.DEFAULT,
    component: PageNotFoundComponent,
    data: {
      title: 'Page Not Found',
    },
  },
];
