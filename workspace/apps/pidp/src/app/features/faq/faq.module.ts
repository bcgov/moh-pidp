import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { SharedUiModule } from '@bcgov/shared/ui';

import { FaqRoutingModule } from './faq-routing.module';

@NgModule({
  imports: [CommonModule, SharedUiModule, FaqRoutingModule],
})
export class FaqModule {}
