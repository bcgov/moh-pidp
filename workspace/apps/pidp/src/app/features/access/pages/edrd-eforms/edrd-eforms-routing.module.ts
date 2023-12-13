import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { EdrdEformsPage } from './edrd-eforms.page';
import { edrdEformsResolver } from './edrd-eforms.resolver';

const routes: Routes = [
  {
    path: '',
    component: EdrdEformsPage,
    resolve: {
      edrdEformsStatusCode: edrdEformsResolver,
    },
    data: {
      title: 'OneHealthID Service',
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
export class EdrdEformsRoutingModule {}
