import {
  ChangeDetectionStrategy,
  Component,
  ContentChildren,
  EventEmitter,
  Input,
  Output,
  QueryList,
  forwardRef,
} from '@angular/core';

import {
  ContextHelpContentDirective,
  ContextHelpTitleDirective,
} from '../../modules/context-help';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { ContextHelpContentDirective as ContextHelpContentDirective_1 } from '../../modules/context-help/context-help-content.directive';
import { ContextHelpTitleDirective as ContextHelpTitleDirective_1 } from '../../modules/context-help/context-help-title.directive';
import { ContextHelpComponent } from '../../modules/context-help/context-help/context-help.component';
import { NgIf } from '@angular/common';

@Component({
    selector: 'ui-form-section',
    templateUrl: './form-section.component.html',
    styleUrls: ['./form-section.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
    standalone: true,
    imports: [
        NgIf,
        ContextHelpComponent,
        ContextHelpTitleDirective_1,
        ContextHelpContentDirective_1,
        MatButtonModule,
        MatIconModule,
        MatTooltipModule,
    ],
})
export class FormSectionComponent {
  /**
   * @description
   * Whether to show the form section action.
   */
  @Input() public showAction: boolean;
  /**
   * @description
   * Action event emitter for removing a control from
   * a list of controls.
   */
  @Output() public action: EventEmitter<void>;

  /**
   * @description
   * Detect whether the section is being used to display contextual
   * help for a specific form section, or removing a control (default)
   */
  @ContentChildren(forwardRef(() => ContextHelpTitleDirective), {
    descendants: false,
  })
  public contextHelpTitleChildren?: QueryList<ContextHelpTitleDirective>;
  @ContentChildren(forwardRef(() => ContextHelpContentDirective), {
    descendants: false,
  })
  public contextHelpContentChildren?: QueryList<ContextHelpContentDirective>;

  public constructor() {
    this.showAction = false;
    this.action = new EventEmitter<void>();
  }

  public onFormSectionAction(): void {
    this.action.emit();
  }
}
