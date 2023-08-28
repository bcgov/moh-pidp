import {
  AbstractControl,
  FormBuilder,
  FormControl,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';

import { AbstractFormState } from '@bcgov/shared/ui';

export interface BcProviderEditFormData {
  newPassword: string;
  confirmPassword: string;
}

export class BcProviderEditFormState extends AbstractFormState<BcProviderEditFormData> {
  public constructor(private fb: FormBuilder) {
    super();

    this.buildForm();
  }

  public get newPassword(): FormControl {
    return this.formInstance.get('newPassword') as FormControl;
  }

  public get confirmPassword(): FormControl {
    return this.formInstance.get('confirmPassword') as FormControl;
  }

  public get json(): BcProviderEditFormData | undefined {
    if (!this.formInstance) {
      return;
    }

    const values = this.formInstance.getRawValue();

    return values;
  }

  public patchValue(model: BcProviderEditFormData | null): void {
    if (!this.formInstance || !model) {
      return;
    }

    this.formInstance.patchValue(model);
  }

  public buildForm(): void {
    this.formInstance = this.fb.group({
      newPassword: ['', [this.validateRequirementsPassword()]],
      confirmPassword: [
        '',
        [Validators.required, this.isEqualToControlValue('newPassword')],
      ],
    });
  }

  public getErrorMessage(): string {
    const errors = this.newPassword.errors;
    if (errors) {
      if (errors.required) {
        return 'Required';
      } else if (errors.uppercase) {
        return 'Must contain at least one uppercase letter';
      } else if (errors.lowercase) {
        return 'Must contain at least one lowercase letter';
      } else if (errors.number) {
        return 'Must contain at least one number';
      } else if (errors.symbol) {
        return 'Must contain at least one symbol';
      } else if (errors.invalidRequirements) {
        return 'Must be between 8 and 256 characters';
      }
    }
    return '';
  }

  private isEqualToControlValue(otherControlName: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.parent) {
        return { isEqualToControlValue: true };
      }
      const thisValue = control.value;
      if (!thisValue) {
        return null;
      }
      const otherControl = control.parent.get(otherControlName);
      const otherValue = otherControl?.value;
      if (!otherValue) {
        return { isEqualToControlValue: true };
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
  private validateRequirementsPassword(): ValidatorFn {
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

      if (!password) return { required: true };
      if (password.length > 7) {
        // Password requirements 3 out of 4 of these to match
        if (upper.test(password)) requirementCounter++;

        if (lower.test(password)) requirementCounter++;

        if (numbers.test(password)) requirementCounter++;

        if (symbols.test(password)) requirementCounter++;
      }

      if (requirementCounter < 3) {
        if (!upper.test(password)) return { uppercase: true };
        if (!lower.test(password)) return { lowercase: true };
        if (!numbers.test(password)) return { number: true };
        if (!symbols.test(password)) return { symbol: true };
        return { invalidRequirements: true };
      }

      return null;
    };
  }
}
