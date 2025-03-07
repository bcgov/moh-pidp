import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';

@Component({
    selector: 'ui-tooltip',
    imports: [CommonModule, MatIconModule],
    templateUrl: './tooltip.component.html',
    styleUrl: './tooltip.component.scss'
})
export class TooltipComponent {
  @Input() public icon?: string;
  @Input() public showTooltip!: boolean;
  @Input() public tooltipText!: string;
  @Input() public buttonLabel: string = '';
  @Output() public actionClicked = new EventEmitter<boolean>();
  @Output() public tooltipStatus = new EventEmitter<boolean>();

  public toggleTooltip(): void {
    this.showTooltip = !this.showTooltip;
    this.tooltipStatus.emit(this.showTooltip);
  }

  public onAction(): void {
    this.actionClicked.next(true);
  }
}
