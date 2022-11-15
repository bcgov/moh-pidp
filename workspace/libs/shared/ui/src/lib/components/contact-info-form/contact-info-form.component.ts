import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'ui-contact-info-form',
  templateUrl: './contact-info-form.component.html',
  styleUrls: ['./contact-info-form.component.scss'],
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
