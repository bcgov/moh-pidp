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
      newPassword: [
        '',
        [
          Validators.required,
          Validators.minLength(8),
          Validators.maxLength(32),
        ],
      ],
      confirmPassword: [
        '',
        [Validators.required, this.isEqualToControlValue('newPassword')],
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
}
