import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

import { IconType } from '../icon/icon.component';
import { MatIconModule } from '@angular/material/icon';
import { NgIf } from '@angular/common';

@Component({
    selector: 'ui-page-subheader',
    templateUrl: './page-subheader.component.html',
    styleUrls: ['./page-subheader.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
    standalone: true,
    imports: [NgIf, MatIconModule],
})
export class PageSubheaderComponent {
  @Input() public icon?: string;
  @Input() public iconType?: IconType;
}
