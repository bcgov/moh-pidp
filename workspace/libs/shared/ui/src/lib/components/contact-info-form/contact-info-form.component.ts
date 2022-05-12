import { Component, Input } from '@angular/core';
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

  public get phone(): FormControl {
    return this.form.get('phone') as FormControl;
  }

  public get email(): FormControl {
    return this.form.get('email') as FormControl;
  }
}
