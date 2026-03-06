import { NgIf } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';

import { IconType } from '../icon/icon.component';

@Component({
    selector: 'ui-page-subheader',
    templateUrl: './page-subheader.component.html',
    styleUrls: ['./page-subheader.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [NgIf, MatIconModule]
})
export class PageSubheaderComponent {
  @Input() public icon?: string;
  @Input() public iconType?: IconType;
}
