import { Routes } from '@angular/router';

import { canDeactivateFormGuard } from '@app/core/guards/can-deactivate-form.guard';
import { collegeLicenceCompletedResolver } from '@app/features/auth/resolvers/college-licence-completed.resolver';

import { CollegeLicenceDeclarationPage } from './college-licence-declaration.page';

export const routes: Routes = [
  {
    path: '',
    component: CollegeLicenceDeclarationPage,
    canDeactivate: [canDeactivateFormGuard],
    resolve: {
      hasCompletedDeclaration: collegeLicenceCompletedResolver,
    },
    data: {
      title: 'OneHealthID Service',
      routes: {
        root: '../../',
      },
    },
  },
];
