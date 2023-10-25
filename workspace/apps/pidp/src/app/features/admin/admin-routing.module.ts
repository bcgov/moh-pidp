import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { adminGuard } from '@app/core/guards/admin.guard';

import { AuthRoutes } from '../auth/auth.routes';
import { AuthenticationGuard } from '../auth/guards/authentication.guard';
import { AdminRoutes } from './admin.routes';
import { PartiesPage } from './pages/parties/parties.page';
import { AdminDashboardComponent } from './shared/components/admin-dashboard/admin-dashboard.component';

const routes: Routes = [
  {
    path: '',
    component: AdminDashboardComponent,
    canActivate: [AuthenticationGuard.canActivate, adminGuard],
    canActivateChild: [AuthenticationGuard.canActivateChild],
    data: {
      routes: {
        auth: AuthRoutes.routePath(AuthRoutes.ADMIN_LOGIN),
      },
    },
    children: [
      {
        path: AdminRoutes.PARTIES,
        component: PartiesPage,
        data: { title: 'OneHealthID Service' },
      },
      {
        path: '',
        redirectTo: AdminRoutes.PARTIES,
        pathMatch: 'full',
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminRoutingModule {}
