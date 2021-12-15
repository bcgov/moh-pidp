import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { CollegeLicenceInformationComponent } from './college-licence-information.component';

const routes: Routes = [
  {
    path: '',
    component: CollegeLicenceInformationComponent,
    data: {
      title: 'Provider Identity Portal',
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CollegeLicenceInformationRoutingModule {}
