import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SignedOrAcceptedDocumentsComponent } from './signed-or-accepted-documents.component';

const routes: Routes = [
  {
    path: '',
    component: SignedOrAcceptedDocumentsComponent,
    data: {
      title: 'Provider Identity Portal',
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SignedOrAcceptedDocumentsRoutingModule {}
