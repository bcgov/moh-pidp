import { NgFor } from '@angular/common';
import { Component, Input } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

import { TrimDirective } from '../../directives/trim.directive';

@Component({
  selector: 'ui-preferred-name-form',
  templateUrl: './preferred-name-form.component.html',
  styleUrls: ['./preferred-name-form.component.scss'],
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NgFor,
    MatFormFieldModule,
    MatInputModule,
    TrimDirective,
  ],
})
export class PreferredNameFormComponent {
  /**
   * @description
   * Preferred name form.
   */
  @Input() public form!: FormGroup;
  /**
   * @description
   * Mode for displaying the form fields as a
   * single or multiple columns.
   */
  @Input() public mode: 'column' | 'columns';

  public formControlConfig: { label: string; name: string }[];

  public constructor() {
    this.mode = 'column';
    this.formControlConfig = [
      { label: 'Preferred First Name', name: 'preferredFirstName' },
      {
        label: 'Preferred Middle Name (Optional)',
        name: 'preferredMiddleName',
      },
      { label: 'Preferred Last Name', name: 'preferredLastName' },
    ];
  }
}
