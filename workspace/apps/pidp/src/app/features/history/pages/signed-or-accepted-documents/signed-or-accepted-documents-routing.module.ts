import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { setDashboardTitleGuard } from '@pidp/presentation';

import { SignedOrAcceptedDocumentsPage } from './signed-or-accepted-documents.page';

const routes: Routes = [
  {
    path: '',
    component: SignedOrAcceptedDocumentsPage,
    canActivate: [setDashboardTitleGuard],
    data: {
      title: 'OneHealthID Service',
      routes: {
        root: '../../',
      },
      setDashboardTitleGuard: {
        titleText: '',
        titleDescriptionText: '',
      },
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SignedOrAcceptedDocumentsRoutingModule {}
