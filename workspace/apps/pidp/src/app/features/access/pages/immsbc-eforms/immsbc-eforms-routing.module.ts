import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SetDashboardTitleGuard } from '@pidp/presentation';

import { ImmsBCEformsPage } from './immsbc-eforms.page';
import { ImmsBCEformsResolver } from './immsbc-eforms.resolver';

const routes: Routes = [
  {
    path: '',
    component: ImmsBCEformsPage,
    canActivate: [SetDashboardTitleGuard],
    resolve: {
      immsBCEformsStatusCode: ImmsBCEformsResolver,
    },
    data: {
      title: 'ImmsBC eForms and OneHealthID',
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
export class ImmsBCEformsRoutingModule {}
