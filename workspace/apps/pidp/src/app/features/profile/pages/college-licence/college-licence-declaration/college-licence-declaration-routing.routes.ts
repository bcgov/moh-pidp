import { Routes } from '@angular/router';

import { setDashboardTitleGuard } from '@pidp/presentation';

import { canDeactivateFormGuard } from '@app/core/guards/can-deactivate-form.guard';
import { highAssuranceCredentialGuard } from '@app/features/auth/guards/high-assurance-credential.guard';
import { collegeLicenceCompletedResolver } from '@app/features/auth/resolvers/college-licence-completed.resolver';

import { CollegeLicenceDeclarationPage } from './college-licence-declaration.page';

export const routes: Routes = [
  {
    path: '',
    component: CollegeLicenceDeclarationPage,
    canActivate: [setDashboardTitleGuard, highAssuranceCredentialGuard],
    canDeactivate: [canDeactivateFormGuard],
    resolve: {
      hasCompletedDeclaration: collegeLicenceCompletedResolver,
    },
    data: {
      title: 'OneHealthID Service',
      routes: {
        root: '../../',
      },
      setDashboardTitleGuard: {
        titleText: 'College Licence Information',
        titleDescriptionText:
          'Provide the following information to complete your Provider Identity Profile',
      },
    },
  },
];
