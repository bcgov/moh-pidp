import {
  ChangeDetectionStrategy,
  Component,
  ContentChildren,
  Input,
  QueryList,
} from '@angular/core';

import { IconType } from '../icon/icon.component';
import { AlertActionsDirective } from './alert-actions.directive';
import { AlertContentDirective } from './alert-content.directive';

export type AlertType = 'success' | 'info' | 'warn' | 'danger' | 'muted';

@Component({
  selector: 'ui-alert',
  templateUrl: './alert.component.html',
  styleUrls: ['./alert.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AlertComponent {
  @Input() public type!: AlertType;
  @Input() public heading!: string;
  @Input() public icon?: string;
  @Input() public iconType?: IconType;

  @ContentChildren(AlertContentDirective)
  public alertContent: QueryList<AlertContentDirective>;
  @ContentChildren(AlertActionsDirective)
  public alertActions: QueryList<AlertActionsDirective>;

  public constructor() {
    // TODO ensure structure and indent applies regardless of existence of type
    this.type = 'muted';
    this.alertContent = new QueryList();
    this.alertActions = new QueryList();
  }
}
