import { FormBuilder, FormControl, Validators } from '@angular/forms';

import { AbstractFormState } from '@bcgov/shared/ui';

import { ExternalAccountsFormData } from './external-accounts.model';

export class ExternalAccountsFormState extends AbstractFormState<ExternalAccountsFormData> {
  public constructor(private fb: FormBuilder) {
    super();

    this.buildForm();
  }

  public get userPrincipalName(): FormControl {
    return this.formInstance.get('userPrincipalName') as FormControl;
  }

  public get json(): ExternalAccountsFormData | undefined {
    if (!this.formInstance) {
      return;
    }

    const values = this.formInstance.getRawValue();

    return values;
  }

  public patchValue(model: ExternalAccountsFormData | null): void {
    if (!this.formInstance || !model) {
      return;
    }

    this.formInstance.patchValue(model);
  }

  public buildForm(): void {
    this.formInstance = this.fb.group({
      userPrincipalName: ['', [Validators.required, Validators.email]],
    });
  }
}
