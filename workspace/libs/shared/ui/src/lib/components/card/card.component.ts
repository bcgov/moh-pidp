import { NgIf } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  ContentChildren,
  Input,
  QueryList,
} from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';

import { IconType } from '../icon/icon.component';
import { CardActionsDirective } from './card-actions.directive';
import { CardContentDirective } from './card-content.directive';
import { CardHintDirective } from './card-hint.directive';

@Component({
    selector: 'ui-card',
    templateUrl: './card.component.html',
    styleUrls: ['./card.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [CardHintDirective, MatCardModule, NgIf, MatIconModule]
})
export class CardComponent {
  @Input() public icon?: string;
  @Input() public iconType?: IconType;
  @Input() public heading!: string;
  @Input() public class?: string | string[] | Record<string, unknown>;

  @ContentChildren(CardHintDirective)
  public cardHint: QueryList<CardHintDirective>;
  @ContentChildren(CardContentDirective)
  public cardContent: QueryList<CardContentDirective>;
  @ContentChildren(CardActionsDirective)
  public cardActions: QueryList<CardActionsDirective>;

  public constructor() {
    this.cardHint = new QueryList();
    this.cardContent = new QueryList();
    this.cardActions = new QueryList();
  }
}
