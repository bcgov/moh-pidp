import { Routes } from '@angular/router';

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
    canActivate: [highAssuranceCredentialGuard],
    data: {
      title: 'OneHealthID Service',
      routes: {
        root: '../../',
      },
    },
  },
];
