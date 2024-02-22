import { Routes } from '@angular/router';

import { canDeactivateFormGuard } from '@app/core/guards/can-deactivate-form.guard';

import { WorkAndRoleInformationPage } from './work-and-role-information.page';

export const routes: Routes = [
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
