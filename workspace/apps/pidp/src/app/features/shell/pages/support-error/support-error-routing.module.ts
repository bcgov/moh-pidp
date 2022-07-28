import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SupportErrorPage } from './support-error.page';

const routes: Routes = [
  {
    path: '',
    component: SupportErrorPage,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SupportErrorRoutingModule {}
