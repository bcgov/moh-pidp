import { NgModule } from '@angular/core';

import { DashboardModule } from '@bcgov/shared/ui';

import { SharedModule } from '@app/shared/shared.module';

import { SupportErrorRoutingModule } from './support-error-routing.module';
import { SupportErrorPage } from './support-error.page';

@NgModule({
  imports: [
    SupportErrorRoutingModule,
    DashboardModule,
    SharedModule,
    SupportErrorPage,
  ],
})
export class SupportErrorModule {}
