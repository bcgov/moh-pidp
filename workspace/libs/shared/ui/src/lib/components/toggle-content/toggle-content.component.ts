import { NgIf } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';
import { ThemePalette } from '@angular/material/core';
import {
  MatSlideToggleChange,
  MatSlideToggleModule,
} from '@angular/material/slide-toggle';

import { ToggleContentChange } from './toggle-content-change.model';

@Component({
  selector: 'ui-toggle-content',
  templateUrl: './toggle-content.component.html',
  styleUrls: ['./toggle-content.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [MatSlideToggleModule, NgIf],
})
export class ToggleContentComponent {
  @Input() public color: ThemePalette;
  @Input() public label!: string;
  @Input() public checked!: boolean;
  @Output() public toggleContent: EventEmitter<ToggleContentChange>;

  public constructor() {
    this.color = 'primary';
    this.toggleContent = new EventEmitter<ToggleContentChange>();
  }

  public onToggleContent({ checked }: MatSlideToggleChange): void {
    this.checked = !this.checked;
    this.toggleContent.emit({ checked });
  }
}
