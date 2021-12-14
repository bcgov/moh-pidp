import { Component, ChangeDetectionStrategy, Input } from '@angular/core';
import { ThemePalette } from '@angular/material/core';

@Component({
  selector: 'ui-key-value-info',
  templateUrl: './key-value-info.component.html',
  styleUrls: ['./key-value-info.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class KeyValueInfoComponent {
  @Input() public key!: string;
}
