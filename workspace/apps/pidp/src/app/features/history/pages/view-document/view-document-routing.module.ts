import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SetDashboardTitleGuard } from '@pidp/presentation';

import { ViewDocumentPage } from './view-document.page';

const routes: Routes = [
  {
    path: '',
    component: ViewDocumentPage,
    canActivate: [SetDashboardTitleGuard],
    data: {
      title: 'Provider Identity Portal',
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
