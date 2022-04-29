import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { FacilityDetailsComponent } from './facility-details.component';

const routes: Routes = [
  {
    path: '',
    component: FacilityDetailsComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class FacilityDetailsRoutingModule {}
