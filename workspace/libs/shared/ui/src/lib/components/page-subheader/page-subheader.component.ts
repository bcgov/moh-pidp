import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

import { IconType } from '../icon/icon.component';

@Component({
  selector: 'ui-page-subheader',
  templateUrl: './page-subheader.component.html',
  styleUrls: ['./page-subheader.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PageSubheaderComponent {
  @Input() public icon?: string;
  @Input() public iconType?: IconType;
}
