import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { FaqRoutingModule } from './faq-routing.module';

@NgModule({
  imports: [SharedModule, FaqRoutingModule],
})
export class FaqModule {}
