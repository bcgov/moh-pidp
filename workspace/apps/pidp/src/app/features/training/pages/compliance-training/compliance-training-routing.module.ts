import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ComplianceTrainingComponent } from './compliance-training.component';

const routes: Routes = [
  {
    path: '',
    component: ComplianceTrainingComponent,
    data: {
      title: 'Provider Identity Portal',
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ComplianceTrainingRoutingModule {}
