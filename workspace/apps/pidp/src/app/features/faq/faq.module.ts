import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { SharedUiModule } from '@bcgov/shared/ui';

import { FaqRoutingModule } from './faq-routing.module';
import { FaqComponent } from './faq.component';

@NgModule({
  imports: [CommonModule, SharedUiModule, FaqRoutingModule],
  declarations: [FaqComponent],
})
export class FaqModule {}
