import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'ui-contact-info-form',
  templateUrl: './contact-info-form.component.html',
  styleUrls: ['./contact-info-form.component.scss'],
})
export class ContactFormComponent implements OnInit {
  /**
   * @description
   * Contact information form instance.
   */
  @Input() public form!: FormGroup;

  public constructor() {}

  public get phone(): FormControl {
    return this.form.get('phone') as FormControl;
  }

  public get email(): FormControl {
    return this.form.get('email') as FormControl;
  }

  public ngOnInit(): void {}
}
