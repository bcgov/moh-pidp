import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

import { IconType } from '../icon/icon.component';

@Component({
  selector: 'ui-page-header',
  templateUrl: './page-header.component.html',
  styleUrls: ['./page-header.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PageHeaderComponent {
  @Input() public icon?: string;
  @Input() public iconType?: IconType;
}
