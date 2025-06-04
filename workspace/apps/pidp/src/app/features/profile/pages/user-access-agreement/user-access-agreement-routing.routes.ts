import { Routes } from '@angular/router';

import { UserAccessAgreementPage } from './user-access-agreement.page';
import { userAccessAgreementResolver } from './user-access-agreement.resolver';

export const userAccessAgreementTitle = `OneHealthID Service Use Policy Agreement`;

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
      },
    },
  },
];
