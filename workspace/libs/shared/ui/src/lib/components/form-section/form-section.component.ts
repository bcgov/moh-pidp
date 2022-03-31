import {
  ChangeDetectionStrategy,
  Component,
  ContentChildren,
  EventEmitter,
  Input,
  Output,
  QueryList,
} from '@angular/core';

import {
  ContextHelpContentDirective,
  ContextHelpTitleDirective,
} from '../../modules';

@Component({
  selector: 'ui-form-section',
  templateUrl: './form-section.component.html',
  styleUrls: ['./form-section.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
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
  @ContentChildren(ContextHelpTitleDirective, { descendants: false })
  public contextHelpTitleChildren?: QueryList<ContextHelpTitleDirective>;
  @ContentChildren(ContextHelpContentDirective, { descendants: false })
  public contextHelpContentChildren?: QueryList<ContextHelpContentDirective>;

  public constructor() {
    this.showAction = false;
    this.action = new EventEmitter<void>();
  }

  public onFormSectionAction(): void {
    this.action.emit();
  }
}
