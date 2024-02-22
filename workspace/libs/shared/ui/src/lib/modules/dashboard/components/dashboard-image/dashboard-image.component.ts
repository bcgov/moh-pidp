import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

@Component({
  selector: 'ui-dashboard-image',
  templateUrl: './dashboard-image.component.html',
  styleUrls: ['./dashboard-image.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
})
export class DashboardImageComponent {
  @Input() public brandConfig!: { imgSrc: string; imgAlt: string };
  @Input() public responsiveMenuItems!: boolean;
}
