import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { EndorsementPage } from './endorsement.page';
import { EndorsementResolver } from './endorsement.resolver';

const routes: Routes = [
  {
    path: '',
    component: EndorsementPage,
    resolve: {
      endorsementStatusCode: EndorsementResolver,
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
