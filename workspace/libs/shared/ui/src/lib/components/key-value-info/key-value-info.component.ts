import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

export type KeyValueInfoOrientation = 'vertical' | 'horizontal';

@Component({
  selector: 'ui-key-value-info',
  templateUrl: './key-value-info.component.html',
  styleUrls: ['./key-value-info.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
})
export class KeyValueInfoComponent {
  @Input() public key!: string;
  @Input() public mode: KeyValueInfoOrientation;

  public constructor() {
    this.mode = 'vertical';
  }
}
