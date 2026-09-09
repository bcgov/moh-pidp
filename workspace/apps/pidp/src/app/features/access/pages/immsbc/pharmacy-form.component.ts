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
  @Input() public isNameReadonly = false;

}
