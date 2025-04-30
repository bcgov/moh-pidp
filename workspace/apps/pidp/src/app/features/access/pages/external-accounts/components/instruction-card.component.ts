import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output, model } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';

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
  @Input() cardData: any;
  @Input() isActive: boolean = false;
  @Input() isCompleted: boolean = false;
  @Input() public routePath = '';
  @Output() continueEvent = new EventEmitter<any>();

  email: string = '';

  public onContinue(value?: any): void {
    console.log('Continue clicked', value);
    this.continueEvent.emit(value);
  }
}
