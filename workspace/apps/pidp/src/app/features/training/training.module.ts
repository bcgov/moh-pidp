import { NgModule } from '@angular/core';

import { SharedModule } from '../../shared/shared.module';

import { ComplianceTrainingComponent } from './pages/compliance-training/compliance-training.component';
import { TrainingRoutingModule } from './training-routing.module';

@NgModule({
  declarations: [ComplianceTrainingComponent],
  imports: [TrainingRoutingModule, SharedModule],
})
export class TrainingModule {}
