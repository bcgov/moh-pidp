import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { PortalComponent } from './portal.component';

const routes: Routes = [
  {
    path: '',
    component: PortalComponent,
    data: { title: 'Provider Identity Portal' },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PortalRoutingModule {}
