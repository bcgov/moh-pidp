import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { UciPage } from './uci.page';
import { UciResolver } from './uci.resolver';

const routes: Routes = [
  {
    path: '',
    component: UciPage,
    resolve: {
      uciStatusCode: UciResolver,
    },
    data: {
      title: 'Provider Identity Portal',
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
export class UciRoutingModule {}
