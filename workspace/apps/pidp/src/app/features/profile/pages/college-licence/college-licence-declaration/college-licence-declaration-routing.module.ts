import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { setDashboardTitleGuard } from '@pidp/presentation';

import { canDeactivateFormGuard } from '@app/core/guards/can-deactivate-form.guard';

import { CollegeLicenceDeclarationPage } from './college-licence-declaration.page';
import { highAssuranceCredentialGuard } from '@app/features/auth/guards/high-assurance-credential.guard';

const routes: Routes = [
  {
    path: '',
    component: CollegeLicenceDeclarationPage,
    canActivate: [setDashboardTitleGuard, highAssuranceCredentialGuard],
    canDeactivate: [canDeactivateFormGuard],
    data: {
      title: 'OneHealthID Service',
      routes: {
        root: '../../',
      },
      setDashboardTitleGuard: {
        titleText: 'College Licence Information',
        titleDescriptionText:
          'Provide the following information to complete your Provider Identity Profile',
      },
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CollegeLicenceDeclarationRoutingModule {}
