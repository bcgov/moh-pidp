import { FormBuilder, FormControl, Validators } from '@angular/forms';

import { AbstractFormState } from '@bcgov/shared/ui';

export interface B2bInvitationFormData {
  userPrincipalName: string;
}

export class B2bInformationFormState extends AbstractFormState<B2bInvitationFormData> {
  public constructor(private fb: FormBuilder) {
    super();

    this.buildForm();
  }

  public get userPrincipalName(): FormControl {
    return this.formInstance.get('userPrincipalName') as FormControl;
  }

  public get json(): B2bInvitationFormData | undefined {
    if (!this.formInstance) {
      return;
    }

    const values = this.formInstance.getRawValue();

    return values;
  }

  public patchValue(model: B2bInvitationFormData | null): void {
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
