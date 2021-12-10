import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule, Routes } from '@angular/router';

import { AccessDeniedComponent } from './pages/access-denied/access-denied.component';
import { MaintenanceComponent } from './pages/maintenance/maintenance.component';
import { PageNotFoundComponent } from './pages/page-not-found/page-not-found.component';
import { rootRouteConfigProvider } from './root-route.config';
import { RootRoutes } from './root.routes';
import { RootRouteContainerComponent } from './shared/root-route-container/root-route-container.component';

export const routes: Routes = [
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

@NgModule({
  declarations: [
    PageNotFoundComponent,
    MaintenanceComponent,
    AccessDeniedComponent,
    RootRouteContainerComponent,
  ],
  imports: [CommonModule, RouterModule.forChild(routes), MatButtonModule],
  exports: [PageNotFoundComponent, MaintenanceComponent, AccessDeniedComponent],
  providers: [rootRouteConfigProvider],
})
export class RootRoutingModule {}
