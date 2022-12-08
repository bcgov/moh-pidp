import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { PidpDataModelModule } from '@pidp/data-model';

@NgModule({
  imports: [CommonModule, PidpDataModelModule],
})
export class PidpPresentationModule {}
