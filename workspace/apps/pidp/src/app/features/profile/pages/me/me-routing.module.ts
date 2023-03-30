import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SetDashboardTitleGuard } from '@pidp/presentation';

import { CanDeactivateFormGuard } from '@app/core/guards/can-deactivate-form.guard';

import { MePage } from './me.page';

const routes: Routes = [
  {
    path: '',
    component: MePage,
    canActivate: [SetDashboardTitleGuard],
    canDeactivate: [CanDeactivateFormGuard],
    data: {
      setDashboardTitleGuard: {
        titleText: 'Provider Identity Portal',
        titleDescriptionText: '',
      },
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MeRoutingModule {}
