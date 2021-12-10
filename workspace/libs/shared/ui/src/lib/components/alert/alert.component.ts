import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  Input,
} from '@angular/core';

import { IconType } from '../icon/icon.component';

export type AlertType = 'success' | 'info' | 'warn' | 'danger' | 'muted';

@Component({
  selector: 'ui-alert',
  templateUrl: './alert.component.html',
  styleUrls: ['./alert.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AlertComponent implements OnInit {
  @Input() title!: string;
  @Input() public type!: AlertType;
  @Input() public icon!: string;
  @Input() public iconType!: IconType;

  public constructor() {}

  public ngOnInit(): void {}
}
