import { NgIf, NgTemplateOutlet } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';
import { MatButtonModule } from '@angular/material/button';

import { AlertType } from '../alert/alert.component';
import { CardActionsDirective } from '../card/card-actions.directive';
import { CardContentDirective } from '../card/card-content.directive';
import { CardHintDirective } from '../card/card-hint.directive';
import { CardComponent } from '../card/card.component';

@Component({
  selector: 'ui-card-summary',
  templateUrl: './card-summary.component.html',
  styleUrls: ['./card-summary.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  imports: [
    CardActionsDirective,
    CardComponent,
    CardContentDirective,
    CardHintDirective,
    MatButtonModule,
    NgIf,
    NgTemplateOutlet,
  ],
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
