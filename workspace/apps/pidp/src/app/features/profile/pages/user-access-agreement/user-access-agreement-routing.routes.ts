import { Routes } from '@angular/router';

import { UserAccessAgreementPage } from './user-access-agreement.page';
import { userAccessAgreementResolver } from './user-access-agreement.resolver';

export const userAccessAgreementTitle = `Access Harmonization User Access Agreement`;

export const routes: Routes = [
  {
    path: '',
    component: UserAccessAgreementPage,
    resolve: {
      userAccessAgreementCode: userAccessAgreementResolver,
    },
    data: {
      title: userAccessAgreementTitle,
      routes: {
        root: '../../',
        setDashboardTitleGuard: {
          titleText: 'Welcome to OneHealthID Service',
          titleDescriptionText:
            'Complete your profile to gain access to the systems you are eligible for',
        },
      },
    },
  },
];
