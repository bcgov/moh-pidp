import { NgIf } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

import { NgxMaskDirective } from 'ngx-mask';

@Component({
  selector: 'ui-contact-info-form',
  templateUrl: './contact-info-form.component.html',
  styleUrls: ['./contact-info-form.component.scss'],
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    NgIf,
    NgxMaskDirective,
  ],
})
export class ContactFormComponent {
  /**
   * @description
   * Contact information form instance.
   */
  @Input() public form!: FormGroup;
  @Output() public emailChanges!: EventEmitter<string>;

  public constructor() {
    this.emailChanges = new EventEmitter<string>();
  }

  public get phone(): FormControl {
    return this.form.get('phone') as FormControl;
  }

  public get email(): FormControl {
    return this.form.get('email') as FormControl;
  }

  public onKeyUp(): void {
    this.emailChanges.emit(this.email.value);
  }
}
