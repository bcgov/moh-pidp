import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { NgxMaskDirective } from 'ngx-mask';

@Component({
  selector: 'app-pharmacy-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NgxMaskDirective],
  templateUrl: './pharmacy-form.component.html',
  styleUrl: './pharmacy-form.component.scss',
})
export class PharmacyFormComponent {
  @Input() public form!: FormGroup;
  @Input() public showAcknowledgements = false;
  @Input() public isNameReadonly = false;

  public onFileChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length) {
      this.form.patchValue({
        evidence: input.files[0]
      });
    }
  }
}
