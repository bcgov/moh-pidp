import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { NextStepsPage } from './pages/next-steps/next-steps.page';
import { SelfDeclarationPage } from './pages/self-declaration/self-declaration.page';
import { TermsOfAccessPage } from './pages/terms-of-access/terms-of-access.page';
import { PharmanetRoutes } from './pharmanet.routes';

const routes: Routes = [
  {
    path: PharmanetRoutes.SELF_DECLARATION,
    component: SelfDeclarationPage,
    data: {
      title: 'Provider Identity Portal',
    },
  },
  {
    path: PharmanetRoutes.TERMS_OF_ACCESS,
    component: TermsOfAccessPage,
    data: {
      title: 'Provider Identity Portal',
    },
  },
  {
    path: PharmanetRoutes.NEXT_STEPS,
    component: NextStepsPage,
    data: {
      title: 'Provider Identity Portal',
    },
  },
  {
    path: '',
    redirectTo: PharmanetRoutes.SELF_DECLARATION,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PharmanetRoutingModule {}
