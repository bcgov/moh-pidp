import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { EndorsementRequestPage } from './endorsement-request.page';
import { EndorsementRequestResolver } from './endorsement-request.resolver';

const routes: Routes = [
  {
    path: '',
    component: EndorsementRequestPage,
    resolve: {
      endorsementStatusCode: EndorsementRequestResolver,
    },
    data: {
      title: 'Endorsement',
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
export class EndorsementRoutingModule {}
