import { FormBuilder, FormControl, Validators } from '@angular/forms';

import { AbstractFormState, FormControlValidators } from '@bcgov/shared/ui';

import { PersonalInformationModel } from './personal-information.model';

export class PersonalInformationFormState extends AbstractFormState<PersonalInformationModel> {
  public constructor(private fb: FormBuilder) {
    super();

    this.buildForm();
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

  public get json(): PersonalInformationModel | undefined {
    if (!this.formInstance) {
      return;
    }

    return this.formInstance.getRawValue();
  }

  public patchValue(model: PersonalInformationModel | null): void {
    if (!this.formInstance || !model) {
      return;
    }

    this.formInstance.patchValue(model);
  }

  public buildForm(): void {
    this.formInstance = this.fb.group({
      preferredFirstName: [null, []],
      preferredMiddleName: [null, []],
      preferredLastName: [null, []],
      phone: [null, [Validators.required, FormControlValidators.phone]],
      email: [null, [Validators.required, FormControlValidators.email]],
    });
  }
}
