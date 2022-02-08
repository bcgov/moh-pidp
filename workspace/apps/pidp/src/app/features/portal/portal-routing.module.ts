import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { PortalPage } from './portal.page';

const routes: Routes = [
  {
    path: '',
    component: PortalPage,
    data: { title: 'Provider Identity Portal' },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PortalRoutingModule {}
