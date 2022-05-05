import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';

import { ContextHelpContentDirective } from './context-help-content.directive';
import { ContextHelpTitleDirective } from './context-help-title.directive';
import { ContextHelpComponent } from './context-help/context-help.component';

@NgModule({
  declarations: [
    ContextHelpComponent,
    ContextHelpTitleDirective,
    ContextHelpContentDirective,
  ],
  imports: [CommonModule, MatButtonModule, MatIconModule, MatMenuModule],
  exports: [
    ContextHelpComponent,
    ContextHelpTitleDirective,
    ContextHelpContentDirective,
  ],
})
export class ContextHelpModule {}
