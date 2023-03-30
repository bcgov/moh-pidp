import { FormBuilder, FormControl, Validators } from '@angular/forms';

import { AbstractFormState, FormControlValidators } from '@bcgov/shared/ui';

import { MeInformation } from './me.model';

export class MeFormState extends AbstractFormState<MeInformation> {
  public constructor(private fb: FormBuilder) {
    super();

    this.buildForm();
  }

  public get email(): FormControl {
    return this.formInstance.get('email') as FormControl;
  }

  public get phone(): FormControl {
    return this.formInstance.get('phone') as FormControl;
  }
  public get preferredFirstName(): FormControl {
    return this.formInstance.get('preferredFirstName') as FormControl;
  }

  public get preferredMiddleName(): FormControl {
    return this.formInstance.get('preferredMiddleName') as FormControl;
  }

  public get preferredLastName(): FormControl {
    return this.formInstance.get('preferredLastName') as FormControl;
  }

  public get collegeCode(): FormControl {
    return this.formInstance.get('collegeCode') as FormControl;
  }

  public get licenceNumber(): FormControl {
    return this.formInstance.get('licenceNumber') as FormControl;
  }

  public get json(): MeInformation | undefined {
    if (!this.formInstance) {
      return;
    }

    const values = this.formInstance.getRawValue();

    // Map '0' in the form back into null for the server (mat-select can't use null as a value)
    if (values.collegeCode === 0) {
      values.collegeCode = null;
      values.licenceNumber = null;
    }

    return values;
  }

  public patchValue(model: MeInformation | null): void {
    if (!this.formInstance || !model) {
      return;
    }

    // Map null from server into '0' in the form (mat-select can't use null as a value)
    if (model.licence.collegeCode === null) {
      model.licence.collegeCode = 0;
    }

    this.formInstance.patchValue(model);
  }

  public buildForm(): void {
    this.formInstance = this.fb.group({
      email: [null, [Validators.required, FormControlValidators.email]],
      phone: [null, [Validators.required, FormControlValidators.phone]],
      preferredFirstName: [null, []],
      preferredMiddleName: [null, []],
      preferredLastName: [null, []],
      collegeCode: [null, [Validators.required]],
      licenceNumber: [null, [Validators.required]],
    });

    if (!this.form.controls.collegeCode.value) {
      this.formInstance.controls.licenceNumber.disable();
    }
  }
}
