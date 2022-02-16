import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AdminGuard } from '@app/core/guards/admin.guard';

import { AuthenticationGuard } from '../auth/guards/authentication.guard';
import { AdminRoutes } from './admin.routes';
import { PartiesPage } from './pages/parties/parties.page';
import { AdminDashboardComponent } from './shared/components/admin-dashboard/admin-dashboard.component';

const routes: Routes = [
  {
    path: '',
    component: AdminDashboardComponent,
    canActivate: [AuthenticationGuard, AdminGuard],
    canActivateChild: [AuthenticationGuard],
    data: {
      // TODO don't hardcode in the redirect URL but also don't want cross module dependencies,
      //      refactor when modules become libs otherwise premature optimization
      routes: {
        auth: '/auth/admin',
      },
    },
    children: [
      {
        path: AdminRoutes.PARTIES,
        component: PartiesPage,
        data: { title: 'Provider Identity Portal' },
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
