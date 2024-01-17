import { NgModule } from '@angular/core';

import { SharedModule } from '@shared/shared.module';

import { ComplianceTrainingRoutingModule } from './compliance-training-routing.module';
import { ComplianceTrainingPage } from './compliance-training.page';

@NgModule({
  imports: [
    ComplianceTrainingRoutingModule,
    SharedModule,
    ComplianceTrainingPage,
  ],
})
export class ComplianceTrainingModule {}
