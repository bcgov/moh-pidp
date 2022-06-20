import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { EndorsementRequestPage } from './pages/endorsement-request/endorsement-request.page';
import { EndorsementRequestResolver } from './pages/endorsement-request/endorsement-request.resolver';

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
