import { Component, ChangeDetectionStrategy, Input } from '@angular/core';

@Component({
  selector: 'ui-card-summary',
  templateUrl: './card-summary.component.html',
  styleUrls: ['./card-summary.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CardSummaryComponent {
  @Input() public icon!: string;
  @Input() public title!: string;
  @Input() public hint?: string;
  @Input() public actionLabel?: string;
  @Input() public statusType?: 'info' | 'warn';
  @Input() public status?: string;
}
