import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'ui-tooltip',
  standalone: true,
  imports: [CommonModule, MatIconModule],
  templateUrl: './tooltip.component.html',
  styleUrl: './tooltip.component.scss',
})
export class TooltipComponent {
  @Input() public icon?: string;
  @Input() public showTooltip!: boolean;
  @Input() public tooltipText!: string;
  @Input() public buttonLabel: string = '';
  @Output() public actionClicked = new EventEmitter<boolean>();

  public hideTooltip(): void {
    this.showTooltip = false;
  }

  public onAction(): void {
    this.actionClicked.emit(true);
  }
}
