import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { DashboardComponent } from '../../modules/dashboard/components/dashboard/dashboard.component';

import { PortalModule } from './portal/portal.module';

const routes: Routes = [
  {
    path: '',
    component: DashboardComponent,
    canActivate: [],
    canActivateChild: [],
    children: [
      {
        path: '',
        loadChildren: (): Promise<PortalModule> =>
          import('./portal/portal.module').then((m) => m.PortalModule),
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ShellRoutingModule {}
