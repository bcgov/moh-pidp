import { FormBuilder, FormControl, Validators } from '@angular/forms';

import { AbstractFormState } from '@bcgov/shared/ui';

export interface BcProviderApplicationFormData {
  username: string;
  password: string;
}
export class BcProviderApplicationFormState extends AbstractFormState<BcProviderApplicationFormData> {
  public constructor(private fb: FormBuilder) {
    super();

    this.buildForm();
  }

  public get username(): FormControl {
    return this.formInstance.get('username') as FormControl;
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
      username: [
        {
          value: 'example@bcproviderlab.ca',
          disabled: true,
        },
        [Validators.required, Validators.email, Validators.maxLength(255)],
      ],
      password: [
        '',
        [
          Validators.required,
          Validators.minLength(8),
          Validators.maxLength(32),
        ],
      ],
    });
  }
}
