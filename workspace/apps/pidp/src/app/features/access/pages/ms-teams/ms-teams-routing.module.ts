import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { MsTeamsPage } from './ms-teams.page';
import { MsTeamsResolver } from './ms-teams.resolver';

const routes: Routes = [
  {
    path: ':pageid',
    component: MsTeamsPage,
    resolve: {
      msTeamsStatusCode: MsTeamsResolver,
    },
    data: {
      title: 'Provider Identity Portal',
      routes: {
        root: '../../',
      },
    },
  },
  {
    path: '**',
    redirectTo: '0',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MsTeamsRoutingModule {}
