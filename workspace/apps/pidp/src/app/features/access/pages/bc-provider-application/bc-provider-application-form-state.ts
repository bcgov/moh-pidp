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
  private upper = /[A-Z]/;
  private lower = /[a-z]/;
  private numbers = /[0-9]/;
  private symbols = /[^A-Za-z0-9]/;
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
        [this.passwordValidator(), this.validateRequirementsPassword()],
      ],
      confirmPassword: [
        '',
        [Validators.required, this.isEqualToControlValue('password')],
      ],
    });
  }

  public getErrorMessage(): string {
    const errors = this.password.errors;
    console.log(errors);
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

  // Validate password to return appropriate validation error for cases
  // required, uppercase, lowercase, number and symbol
  private passwordValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      // hide validation errors until control is dirty
      if (control.pristine) {
        return null;
      }

      const password = control.value;

      if (!password) return { required: true };
      if (!this.upper.test(password)) return { uppercase: true };
      if (!this.lower.test(password)) return { lowercase: true };
      if (!this.numbers.test(password)) return { number: true };
      if (!this.symbols.test(password)) return { symbol: true };

      return null;
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
      let requirementCounter = 0;

      if (password.length > 7) {
        // Password requirements 3 out of 4 of these to match
        if (this.upper.test(password)) requirementCounter++;

        if (this.lower.test(password)) requirementCounter++;

        if (this.numbers.test(password)) requirementCounter++;

        if (this.symbols.test(password)) requirementCounter++;
      }

      if (requirementCounter < 3) {
        return { invalidRequirements: true };
      }

      return null;
    };
  }
}
