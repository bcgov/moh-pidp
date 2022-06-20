import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { UserAccessAgreementPage } from './user-access-agreement.page';

const routes: Routes = [
  {
    path: '',
    component: UserAccessAgreementPage,
    data: {
      title: 'Access Harmonization User Access Agreement',
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
export class UserAccessAgreementRoutingModule {}
