import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

// import { AppRoutes } from './app.routes';

const routes: Routes = [
  // TODO turn on root routes
  // {
  //   path: AppRoutes.MAINTENANCE,
  //   component: MaintenanceComponent,
  //   data: {
  //     title: 'Under Scheduled Maintenance',
  //   },
  // },
  // {
  //   // Allow for direct routing to page not found
  //   path: AppRoutes.PAGE_NOT_FOUND,
  //   component: PageNotFoundComponent,
  //   data: {
  //     title: 'Page Not Found',
  //   },
  // },
  // {
  //   path: AppRoutes.DEFAULT,
  //   component: PageNotFoundComponent,
  //   data: {
  //     title: 'Page Not Found',
  //   },
  // },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      scrollPositionRestoration: 'top',
      enableTracing: false,
    }),
  ],
  exports: [RouterModule],
})
export class AppRoutingModule {}
