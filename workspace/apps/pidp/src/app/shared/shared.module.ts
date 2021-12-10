import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { SharedUiModule } from '@bcgov/shared/ui';

const modules = [CommonModule, SharedUiModule];

@NgModule({
  imports: modules,
  exports: modules,
})
export class SharedModule {}
