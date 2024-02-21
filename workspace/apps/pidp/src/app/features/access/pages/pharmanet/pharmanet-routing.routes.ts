import { Routes } from '@angular/router';

import { NextStepsPage } from './pages/next-steps/next-steps.page';
import { SelfDeclarationPage } from './pages/self-declaration/self-declaration.page';
import { TermsOfAccessPage } from './pages/terms-of-access/terms-of-access.page';
import { PharmanetRoutes } from './pharmanet.routes';

export const routes: Routes = [
  {
    path: PharmanetRoutes.SELF_DECLARATION,
    component: SelfDeclarationPage,
    data: {
      title: 'OneHealthID Service',
    },
  },
  {
    path: PharmanetRoutes.TERMS_OF_ACCESS,
    component: TermsOfAccessPage,
    data: {
      title: 'OneHealthID Service',
    },
  },
  {
    path: PharmanetRoutes.NEXT_STEPS,
    component: NextStepsPage,
    data: {
      title: 'OneHealthID Service',
    },
  },
  {
    path: '',
    redirectTo: PharmanetRoutes.SELF_DECLARATION,
    pathMatch: 'full',
  },
];
