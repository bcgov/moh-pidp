import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { setDashboardTitleGuard } from '@pidp/presentation';

import { CollegeLicenceInformationPage } from './college-licence-information.page';
import { highAssuranceCredentialGuard } from '@app/features/auth/guards/high-assurance-credential.guard';

const routes: Routes = [
  {
    path: '',
    component: CollegeLicenceInformationPage,
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
