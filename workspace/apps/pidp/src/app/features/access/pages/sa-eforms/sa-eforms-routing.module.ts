import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SaEformsPage } from './sa-eforms.page';
import { SaEformsResolver } from './sa-eforms.resolver';

const routes: Routes = [
  {
    path: '',
    component: SaEformsPage,
    resolve: {
      saEformsStatusCode: SaEformsResolver,
    },
    data: {
      title: 'Provider Identity Portal',
      routes: {
        root: '../../',
      },
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SaEformsRoutingModule {}
