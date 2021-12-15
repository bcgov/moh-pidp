import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SpecialAuthorityEformsComponent } from './special-authority-eforms.component';

const routes: Routes = [
  {
    path: '',
    component: SpecialAuthorityEformsComponent,
    data: {
      title: 'Provider Identity Portal',
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SpecialAuthorityEformsRoutingModule {}
