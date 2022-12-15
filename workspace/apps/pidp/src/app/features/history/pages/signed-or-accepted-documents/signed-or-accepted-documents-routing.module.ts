import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SetDashboardTitleGuard } from '@pidp/presentation';

import { SignedOrAcceptedDocumentsPage } from './signed-or-accepted-documents.page';

const routes: Routes = [
  {
    path: '',
    component: SignedOrAcceptedDocumentsPage,
    canActivate: [SetDashboardTitleGuard],
    data: {
      title: 'Provider Identity Portal',
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
