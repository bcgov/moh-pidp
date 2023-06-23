import { FormBuilder } from '@angular/forms';

import { AbstractFormState } from '@bcgov/shared/ui';

export class UserAccessAgreementFormState extends AbstractFormState<void> {
  public constructor(private fb: FormBuilder) {
    super();

    this.buildForm();
  }

  public get json(): void | undefined {
    if (!this.formInstance) {
      return;
    }

    return this.formInstance.getRawValue();
  }

  public patchValue(model: any | null): void {
    if (!this.formInstance) {
      return;
    }

    this.formInstance.patchValue(model);
  }

  public buildForm(): void {
    this.formInstance = this.fb.group({});
  }
}
