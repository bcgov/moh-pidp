import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { EndorsementRequestsReceivedPage } from './endorsement-requests-received.page';
import { EndorsementRequestsReceivedResolver } from './endorsement-requests-received.resolver';

const routes: Routes = [
  {
    path: '',
    component: EndorsementRequestsReceivedPage,
    resolve: {
      endorsementRequestsReceivedStatusCode:
        EndorsementRequestsReceivedResolver,
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
export class EndorsementRequestsReceivedRoutingModule {}
