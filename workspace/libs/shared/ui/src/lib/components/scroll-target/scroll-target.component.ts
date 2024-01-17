import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

@Component({
  selector: 'ui-scroll-target',
  templateUrl: './scroll-target.component.html',
  styleUrls: ['./scroll-target.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
})
export class ScrollTargetComponent {
  @Input() public targetId!: string;
}
