import { NgModule } from '@angular/core';

import { SharedModule } from '../../../../shared/shared.module';

import { ComplianceTrainingRoutingModule } from './compliance-training-routing.module';
import { ComplianceTrainingComponent } from './compliance-training.component';

@NgModule({
  declarations: [ComplianceTrainingComponent],
  imports: [ComplianceTrainingRoutingModule, SharedModule],
})
export class ComplianceTrainingModule {}
