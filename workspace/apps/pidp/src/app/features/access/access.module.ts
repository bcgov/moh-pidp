import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { AccessRoutingModule } from './access-routing.module';
import { EnrolmentErrorComponent } from './components/enrolment-error/enrolment-error.component';

@NgModule({
  imports: [AccessRoutingModule, SharedModule],
  declarations: [EnrolmentErrorComponent],
  exports: [EnrolmentErrorComponent],
})
export class AccessModule {}
