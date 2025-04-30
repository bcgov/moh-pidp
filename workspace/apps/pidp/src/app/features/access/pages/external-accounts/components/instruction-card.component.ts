import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';

import { InstructionCard } from './instruction-card.model';

@Component({
  selector: 'app-instruction-card',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatFormFieldModule,
    MatSelectModule,
    MatIconModule,
  ],
  templateUrl: './instruction-card.component.html',
  styleUrl: './instruction-card.component.scss',
})
export class InstructionCardComponent {
  @Input()
  public cardData: InstructionCard = {} as InstructionCard;

  @Input()
  public isActive = false;

  @Input()
  public isCompleted = false;

  @Output()
  public continueEvent = new EventEmitter<string>();

  public email = '';

  public onContinue(value?: string): void {
    this.continueEvent.emit(value);
  }
}
