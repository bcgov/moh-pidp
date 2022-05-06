import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';
import { ThemePalette } from '@angular/material/core';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';

import { ToggleContentChange } from './toggle-content-change.model';

@Component({
  selector: 'ui-toggle-content',
  templateUrl: './toggle-content.component.html',
  styleUrls: ['./toggle-content.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ToggleContentComponent {
  @Input() public color: ThemePalette;
  @Input() public label!: string;
  @Input() public checked!: boolean;
  @Output() public toggle: EventEmitter<ToggleContentChange>;

  public constructor() {
    this.color = 'primary';
    this.toggle = new EventEmitter<ToggleContentChange>();
  }

  public onToggleContent({ checked }: MatSlideToggleChange): void {
    this.checked = !this.checked;
    this.toggle.emit({ checked });
  }
}
