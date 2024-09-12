import { Routes } from '@angular/router';

import { highAssuranceCredentialGuard } from '@app/features/auth/guards/high-assurance-credential.guard';

import { EndorsementsPage } from './endorsements.page';

export const routes: Routes = [
  {
    path: '',
    component: EndorsementsPage,
    canActivate: [highAssuranceCredentialGuard],
  },
];
