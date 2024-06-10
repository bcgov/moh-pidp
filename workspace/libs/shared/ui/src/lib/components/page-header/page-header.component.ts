import { NgIf } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';

import { IconType } from '../icon/icon.component';

@Component({
  selector: 'ui-page-header',
  templateUrl: './page-header.component.html',
  styleUrls: ['./page-header.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  imports: [NgIf, MatIconModule],
})
export class PageHeaderComponent {
  @Input() public icon?: string;
  @Input() public iconType?: IconType;
}
