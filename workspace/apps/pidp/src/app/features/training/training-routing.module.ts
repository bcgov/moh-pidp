import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ComplianceTrainingModule } from './pages/compliance-training/compliance-training.module';
import { TrainingRoutes } from './training.routes';

const routes: Routes = [
  {
    path: TrainingRoutes.COMPLIANCE_TRAINING,
    loadChildren: (): Promise<ComplianceTrainingModule> =>
      import('./pages/compliance-training/compliance-training.module').then(
        (m) => m.ComplianceTrainingModule
      ),
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TrainingRoutingModule {}
