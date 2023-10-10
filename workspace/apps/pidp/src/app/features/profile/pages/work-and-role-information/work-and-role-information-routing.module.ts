import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { canDeactivateFormGuard } from '@app/core/guards/can-deactivate-form.guard';

import { WorkAndRoleInformationPage } from './work-and-role-information.page';

const routes: Routes = [
  {
    path: '',
    component: WorkAndRoleInformationPage,
    canDeactivate: [canDeactivateFormGuard],
    data: {
      title: 'OneHealthID Service',
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
