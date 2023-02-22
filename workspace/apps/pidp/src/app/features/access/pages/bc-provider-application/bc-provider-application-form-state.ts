import {
  AbstractControl,
  FormBuilder,
  FormControl,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';

import { AbstractFormState } from '@bcgov/shared/ui';

export interface BcProviderApplicationFormData {
  password: string;
}

export class BcProviderApplicationFormState extends AbstractFormState<BcProviderApplicationFormData> {
  public constructor(private fb: FormBuilder) {
    super();

    this.buildForm();
  }

  public get password(): FormControl {
    return this.formInstance.get('password') as FormControl;
  }

  public get confirmPassword(): FormControl {
    return this.formInstance.get('confirmPassword') as FormControl;
  }

  public get json(): BcProviderApplicationFormData | undefined {
    if (!this.formInstance) {
      return;
    }

    const values = this.formInstance.getRawValue();

    return values;
  }

  public patchValue(model: BcProviderApplicationFormData | null): void {
    if (!this.formInstance || !model) {
      return;
    }

    this.formInstance.patchValue(model);
  }

  public buildForm(): void {
    this.formInstance = this.fb.group({
      password: [
        '',
        [
          Validators.required,
          Validators.minLength(8),
          Validators.maxLength(256),
          this.validatePassword(),
        ],
      ],
      confirmPassword: [
        '',
        [Validators.required, this.isEqualToControlValue('password')],
      ],
    });
  }

  private isEqualToControlValue(otherControlName: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.parent) {
        return null;
      }
      const thisValue = control.value;
      if (!thisValue) {
        return null;
      }
      const otherControl = control.parent.get(otherControlName);
      const otherValue = otherControl?.value;
      if (!otherValue) {
        return null;
      }

      const areEqual = thisValue === otherValue;
      if (areEqual) {
        return null;
      }
      return { isEqualToControlValue: true };
    };
  }

  // Password requirements as per Azure Active Directory
  // https://learn.microsoft.com/en-us/azure/active-directory/authentication/concept-sspr-policy
  private validatePassword(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      // hide validation errors until control is dirty
      if (control.pristine) {
        return null;
      }

      const password = control.value;
      const upper = /[A-Z]/;
      const lower = /[a-z]/;
      const numbers = /[0-9]/;
      const symbols = /[^A-Za-z0-9]/;

      let requirementCounter = 0;

      if (password.length > 7) {
        // Password requirements 3 out of 4 of these to match
        if (upper.test(password)) requirementCounter++;

        if (lower.test(password)) requirementCounter++;

        if (numbers.test(password)) requirementCounter++;

        if (symbols.test(password)) requirementCounter++;
      }

      if (requirementCounter < 3) {
        return { invalidRequirements: true };
      }

      return null;
    };
  }
}
