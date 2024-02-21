import { Routes } from '@angular/router';

import { setDashboardTitleGuard } from '@pidp/presentation';

import { canDeactivateFormGuard } from '@app/core/guards/can-deactivate-form.guard';

import { PersonalInformationPage } from './personal-information.page';

export const routes: Routes = [
  {
    path: '',
    component: PersonalInformationPage,
    canActivate: [setDashboardTitleGuard],
    canDeactivate: [canDeactivateFormGuard],
    data: {
      title: 'OneHealthID Service',
      routes: {
        root: '../../',
      },
      setDashboardTitleGuard: {
        titleText: '',
        titleDescriptionText: '',
      },
    },
  },
];