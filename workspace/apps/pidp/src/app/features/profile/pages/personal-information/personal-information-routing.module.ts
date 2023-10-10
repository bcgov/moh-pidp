import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SetDashboardTitleGuard } from '@pidp/presentation';

import { canDeactivateFormGuard } from '@app/core/guards/can-deactivate-form.guard';

import { PersonalInformationPage } from './personal-information.page';

const routes: Routes = [
  {
    path: '',
    component: PersonalInformationPage,
    canActivate: [SetDashboardTitleGuard],
    canDeactivate: [canDeactivateFormGuard],
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
export class PersonalInformationRoutingModule {}
