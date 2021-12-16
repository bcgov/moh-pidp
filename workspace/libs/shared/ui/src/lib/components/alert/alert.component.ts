import { Component, ChangeDetectionStrategy, Input } from '@angular/core';

import { IconType } from '../icon/icon.component';

export type AlertType = 'success' | 'info' | 'warn' | 'danger' | 'muted';

@Component({
  selector: 'ui-alert',
  templateUrl: './alert.component.html',
  styleUrls: ['./alert.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AlertComponent {
  @Input() public type!: AlertType;
  @Input() public title!: string;
  @Input() public icon?: string;
  @Input() public iconType?: IconType;

  public constructor() {
    // TODO ensure structure and indent applies regardless of existence of type
    this.type = 'muted';
  }
}
