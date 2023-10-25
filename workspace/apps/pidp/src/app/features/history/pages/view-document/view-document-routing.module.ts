import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { setDashboardTitleGuard } from '@pidp/presentation';

import { ViewDocumentPage } from './view-document.page';

const routes: Routes = [
  {
    path: '',
    component: ViewDocumentPage,
    canActivate: [setDashboardTitleGuard],
    data: {
      title: 'OneHealthID Service',
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
export class ViewDocumentRoutingModule {}
