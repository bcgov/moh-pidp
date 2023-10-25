import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { setDashboardTitleGuard } from '@pidp/presentation';

import { SaEformsPage } from './sa-eforms.page';
import { SaEformsResolver } from './sa-eforms.resolver';

const routes: Routes = [
  {
    path: '',
    component: SaEformsPage,
    canActivate: [setDashboardTitleGuard],
    resolve: {
      saEformsStatusCode: SaEformsResolver,
    },
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
export class SaEformsRoutingModule {}
