import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';

import { AlertType } from '../alert/alert.component';

@Component({
  selector: 'ui-card-summary',
  templateUrl: './card-summary.component.html',
  styleUrls: ['./card-summary.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CardSummaryComponent {
  @Input() public icon?: string;
  @Input() public heading!: string;
  @Input() public statusType?: AlertType;
  @Input() public status?: string;
  @Input() public actionLabel?: string;
  @Input() public actionDisabled?: boolean;
  @Output() public action: EventEmitter<void>;

  public constructor() {
    this.action = new EventEmitter<void>();
  }

  public onAction(): void {
    this.action.emit();
  }
}
