import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { CanDeactivateFormGuard } from '@app/core/guards/can-deactivate-form.guard';

import { WorkAndRoleInformationComponent } from './work-and-role-information.component';

const routes: Routes = [
  {
    path: '',
    component: WorkAndRoleInformationComponent,
    canDeactivate: [CanDeactivateFormGuard],
    data: {
      title: 'Provider Identity Portal',
      routes: {
        root: '../../',
      },
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class WorkAndRoleInformationRoutingModule {}
