import { NgModule } from '@angular/core';

import { DashboardModule } from '@bcgov/shared/ui';

import { SharedModule } from '@app/shared/shared.module';

import { SupportErrorRoutingModule } from './support-error-routing.module';
import { SupportErrorPage } from './support-error.page';

@NgModule({
  declarations: [SupportErrorPage],
  imports: [SupportErrorRoutingModule, DashboardModule, SharedModule],
})
export class SupportErrorModule {}
