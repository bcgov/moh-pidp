import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { UserAccessAgreementPage } from './user-access-agreement.page';
import { userAccessAgreementResolver } from './user-access-agreement.resolver';

export const userAccessAgreementTitle = `Access Harmonization User Access Agreement`;

const routes: Routes = [
  {
    path: '',
    component: UserAccessAgreementPage,
    resolve: {
      userAccessAgreementCode: userAccessAgreementResolver,
    },
    data: {
      title: userAccessAgreementTitle,
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
