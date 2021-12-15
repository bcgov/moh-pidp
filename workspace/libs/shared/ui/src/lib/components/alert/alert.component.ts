import {
  Component,
  ChangeDetectionStrategy,
  Input,
  ContentChildren,
  QueryList,
} from '@angular/core';

import { IconType } from '../icon/icon.component';
import { AlertActionsDirective } from './alert-actions.directive';
import { AlertContentDirective } from './alert-content.directive';
import { AlertTitleDirective } from './alert-title.directive';

export type AlertType = 'success' | 'info' | 'warn' | 'danger' | 'muted';

@Component({
  selector: 'ui-alert',
  templateUrl: './alert.component.html',
  styleUrls: ['./alert.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AlertComponent {
  @Input() public type!: AlertType;
  @Input() public icon?: string;
  @Input() public iconType?: IconType;

  @ContentChildren(AlertTitleDirective)
  public alertTitle: QueryList<AlertTitleDirective>;
  @ContentChildren(AlertContentDirective)
  public alertContent: QueryList<AlertContentDirective>;
  @ContentChildren(AlertActionsDirective)
  public alertActions: QueryList<AlertActionsDirective>;

  public constructor() {
    // TODO drop default and updates styles to apply structure without type theming
    this.type = 'muted';
    this.alertTitle = new QueryList();
    this.alertContent = new QueryList();
    this.alertActions = new QueryList();
  }
}
