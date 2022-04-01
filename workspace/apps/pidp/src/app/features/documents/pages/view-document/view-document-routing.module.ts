import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ViewDocumentPage } from './view-document.page';

const routes: Routes = [
  {
    path: '',
    component: ViewDocumentPage,
    data: {
      title: 'Provider Identity Portal',
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ViewDocumentRoutingModule {}
