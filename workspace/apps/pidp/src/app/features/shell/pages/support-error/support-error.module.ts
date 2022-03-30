import { NgModule } from '@angular/core';

import { DashboardModule, MaterialModule } from '@bcgov/shared/ui';

import { SupportErrorRoutingModule } from './support-error-routing.module';
import { SupportErrorPage } from './support-error.page';

@NgModule({
  declarations: [SupportErrorPage],
  imports: [SupportErrorRoutingModule, DashboardModule, MaterialModule],
})
export class SupportErrorModule {}
