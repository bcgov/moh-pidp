import { Routes } from '@angular/router';

import { setDashboardTitleGuard } from '@pidp/presentation';

import { highAssuranceCredentialGuard } from '@app/features/auth/guards/high-assurance-credential.guard';

import { EndorsementsPage } from './endorsements.page';

export const routes: Routes = [
  {
    path: '',
    component: EndorsementsPage,
    canActivate: [setDashboardTitleGuard, highAssuranceCredentialGuard],
    data: {
      setDashboardTitleGuard: {
        titleText: 'Welcome to your endorsements',
        titleDescriptionText: '',
      },
    },
  },
];
