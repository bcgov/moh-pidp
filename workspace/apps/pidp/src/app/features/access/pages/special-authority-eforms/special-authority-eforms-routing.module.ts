import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SpecialAuthorityEformsPage } from './special-authority-eforms.page';

const routes: Routes = [
  {
    path: '',
    component: SpecialAuthorityEformsPage,
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
export class SpecialAuthorityEformsRoutingModule {}
