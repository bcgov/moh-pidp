import { Routes } from '@angular/router';

import { setDashboardTitleGuard } from '@pidp/presentation';

import { highAssuranceCredentialGuard } from '@app/features/auth/guards/high-assurance-credential.guard';
import { collegeLicenceInfoResolver } from '@app/features/auth/resolvers/college-licence-info.resolver';

import { CollegeLicenceInformationPage } from './college-licence-information.page';

export const routes: Routes = [
  {
    path: '',
    component: CollegeLicenceInformationPage,
    resolve: {
      hasCpn: collegeLicenceInfoResolver,
    },
    canActivate: [setDashboardTitleGuard, highAssuranceCredentialGuard],
    data: {
      title: 'OneHealthID Service',
      routes: {
        root: '../../',
      },
      setDashboardTitleGuard: {
        titleText: 'Welcome to OneHealthID Service',
        titleDescriptionText:
          'Complete your profile to gain access to the systems you are eligible for',
      },
    },
  },
];
