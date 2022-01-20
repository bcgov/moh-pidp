import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AdminRoutes } from './admin.routes';
import { PartiesComponent } from './pages/parties/parties.component';

const routes: Routes = [
  {
    path: AdminRoutes.PARTIES,
    component: PartiesComponent,
    data: { title: 'Provider Identity Portal' },
  },
  {
    path: '',
    redirectTo: AdminRoutes.PARTIES,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminRoutingModule {}
