import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { setDashboardTitleGuard } from '@pidp/presentation';

import { highAssuranceCredentialGuard } from '@app/features/auth/guards/high-assurance-credential.guard';
import { collegeLicenceInfoResolver } from '@app/features/auth/resolvers/college-licence-info.resolver';

import { CollegeLicenceInformationPage } from './college-licence-information.page';

const routes: Routes = [
  {
    path: '',
    component: CollegeLicenceInformationPage,
    resolve: {
      hasCpn: collegeLicenceInfoResolver,
    },
    canActivate: [setDashboardTitleGuard, highAssuranceCredentialGuard],
    data: {
      title: 'OneHealthID Service',
      routes: {
        root: '../../',
      },
      setDashboardTitleGuard: {
        titleText: '',
        titleDescriptionText: '',
      },
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CollegeLicenceInformationRoutingModule {}
