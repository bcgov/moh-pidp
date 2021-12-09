import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ComplianceTrainingComponent } from './pages/compliance-training/compliance-training.component';

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
export class TrainingRoutingModule {}
