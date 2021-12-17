import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

@Component({
  selector: 'ui-anchor-link',
  templateUrl: './anchor-link.component.html',
  styleUrls: ['./anchor-link.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AnchorLinkComponent {
  @Input() public linkId!: string;
}
