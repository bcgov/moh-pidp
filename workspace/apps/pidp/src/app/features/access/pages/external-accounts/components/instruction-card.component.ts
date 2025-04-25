import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-instruction-card',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatSelectModule,
    MatIconModule,
  ],
  templateUrl: './instruction-card.component.html',
  styleUrl: './instruction-card.component.scss',
})
export class InstructionCardComponent {
  @Input() data!: {
    id: number;
    title: string;
    description: string;
    icon: string;
    content: {
      type: string;
      placeholder?: string;
      helpText?: string;
      buttonText?: string;
    };
    isCompleted: boolean;
    isActive: boolean;
  };

  domainSelectionForm!: FormGroup;

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.domainSelectionForm = this.fb.group({
      domain: ['', Validators.required],
    });
  }

  onSubmit(): void {
    if (this.domainSelectionForm.valid) {
      // Handle form submission
      console.log(this.domainSelectionForm.value);
    }
  }

  showOrganizationHelp(event: Event): void {
    event.preventDefault();
    // Logic to handle "Don't see your organization?" click
    // Could open a modal or navigate to help page
  }

  currentStep = 0;
  selectedDomain = 'Yukon';
  username = '';
}
