import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';

/**
 * @description
 * Set of icon types provided by Material.
 *
 * NOTE: Defaulted type is "filled", which is the
 * absence of the other icon types.
 */
export type IconType = 'filled' | 'outlined' | 'round' | 'sharp';

@Component({
  selector: 'ui-icon',
  templateUrl: './icon.component.html',
  styleUrls: ['./icon.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  imports: [MatIconModule],
})
export class IconComponent {
  @Input() public color: 'primary' | 'accent' | 'warn';
  @Input() public type?: IconType;
  @Input() public size?: 'sm' | 'md' | 'lg';

  public constructor() {
    this.color = 'primary';
  }
}
