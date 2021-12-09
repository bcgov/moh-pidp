import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { NextStepsComponent } from './pages/next-steps/next-steps.component';
import { SelfDeclarationComponent } from './pages/self-declaration/self-declaration.component';
import { TermsOfAccessComponent } from './pages/terms-of-access/terms-of-access.component';
import { PharmanetRoutes } from './pharmanet.routes';

const routes: Routes = [
  {
    path: PharmanetRoutes.SELF_DECLARATION_PAGE,
    component: SelfDeclarationComponent,
    data: {
      title: 'Provider Identity Portal',
    },
  },
  {
    path: PharmanetRoutes.TERMS_OF_ACCESS_PAGE,
    component: TermsOfAccessComponent,
    data: {
      title: 'Provider Identity Portal',
    },
  },
  {
    path: PharmanetRoutes.NEXT_STEPS_PAGE,
    component: NextStepsComponent,
    data: {
      title: 'Provider Identity Portal',
    },
  },
  {
    path: '',
    redirectTo: PharmanetRoutes.SELF_DECLARATION_PAGE,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PharmanetRoutingModule {}
