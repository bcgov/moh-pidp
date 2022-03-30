import { NgModule } from '@angular/core';

import { DashboardModule, MaterialModule } from '@bcgov/shared/ui';

import { SupportErrorPage } from './support-error.page';

@NgModule({
  declarations: [SupportErrorPage],
  imports: [DashboardModule, MaterialModule],
})
export class SupportErrorModule {}
