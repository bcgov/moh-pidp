import { FormBuilder, FormControl, Validators } from '@angular/forms';

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
          Validators.maxLength(32),
        ],
      ],
    });
  }
}
