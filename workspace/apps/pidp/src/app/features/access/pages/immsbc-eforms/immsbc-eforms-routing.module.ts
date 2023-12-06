import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { setDashboardTitleGuard } from '@pidp/presentation';

import { ImmsBCEformsPage } from './immsbc-eforms.page';
import { immsBCEformsResolver } from './immsbc-eforms.resolver';

const routes: Routes = [
  {
    path: '',
    component: ImmsBCEformsPage,
    canActivate: [setDashboardTitleGuard],
    resolve: {
      immsBCEformsStatusCode: immsBCEformsResolver,
    },
    data: {
      title: 'Immunization Entry eForm and OneHealthID',
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
