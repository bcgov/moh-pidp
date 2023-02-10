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
    });
  }

  public validatePassword(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const password = control.value;
      const upper = /[A-Z]/;
      const lower = /[a-z]/;
      const numbers = /[0-9]/;
      const symbols = /[^A-Za-z0-9]/;

      let requirementCounter = 0;
      console.log(password);
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
