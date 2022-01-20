import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ViewDocumentComponent } from './view-document.component';

const routes: Routes = [
  {
    path: '',
    component: ViewDocumentComponent,
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
