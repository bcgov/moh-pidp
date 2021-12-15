import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { WorkAndRoleInformationComponent } from './work-and-role-information.component';

const routes: Routes = [
  {
    path: '',
    component: WorkAndRoleInformationComponent,
    data: {
      title: 'Provider Identity Portal',
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class WorkAndRoleInformationRoutingModule {}
