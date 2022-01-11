import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';

import { Address } from '@bcgov/shared/data-access';
import { AbstractFormState, FormControlValidators } from '@bcgov/shared/ui';

import { PersonalInformationModel } from './personal-information.model';

// TODO add in validation toggles for forms
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

  public get mailingAddress(): FormGroup {
    return this.formInstance.get('mailingAddress') as FormGroup;
  }

  public get phone(): FormControl {
    return this.formInstance.get('phone') as FormControl;
  }

  public get email(): FormControl {
    return this.formInstance.get('email') as FormControl;
  }

  public get json(): PersonalInformationModel | undefined {
    if (!this.formInstance) {
      return;
    }

    const model = this.formInstance.getRawValue();
    if (Address.isEmpty(model.mailingAddress)) {
      model.mailingAddress = null;
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
      mailingAddress: this.fb.group({
        countryCode: [{ value: null, disabled: false }, []],
        provinceCode: [{ value: null, disabled: false }, []],
        street: [{ value: null, disabled: false }, []],
        city: [{ value: null, disabled: false }, []],
        postal: [{ value: null, disabled: false }, []],
      }),
      phone: [null, [Validators.required, FormControlValidators.phone]],
      email: [null, [Validators.required, FormControlValidators.email]],
    });
  }
}
