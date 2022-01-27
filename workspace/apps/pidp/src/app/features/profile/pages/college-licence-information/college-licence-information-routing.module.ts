import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { CanDeactivateFormGuard } from '@app/core/guards/can-deactivate-form.guard';

import { CollegeLicenceInformationComponent } from './college-licence-information.component';

const routes: Routes = [
  {
    path: '',
    component: CollegeLicenceInformationComponent,
    canDeactivate: [CanDeactivateFormGuard],
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
export class CollegeLicenceInformationRoutingModule {}
