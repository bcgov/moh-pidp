import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

@Component({
  selector: 'ui-key-value-info',
  templateUrl: './key-value-info.component.html',
  styleUrls: ['./key-value-info.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class KeyValueInfoComponent {
  @Input() public key!: string;
  @Input() public mode: 'vertical' | 'horizontal';

  public constructor() {
    this.mode = 'vertical';
  }
}
