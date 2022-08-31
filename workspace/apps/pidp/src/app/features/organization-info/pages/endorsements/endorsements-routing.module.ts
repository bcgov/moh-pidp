import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { EndorsementsPage } from './endorsements.page';

const routes: Routes = [
  {
    path: '',
    component: EndorsementsPage,
    data: {
      title: 'Your Care Team',
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
export class EndorsementsRoutingModule {}
