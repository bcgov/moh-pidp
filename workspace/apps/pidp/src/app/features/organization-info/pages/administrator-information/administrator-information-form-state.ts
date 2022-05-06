import { FormBuilder, Validators } from '@angular/forms';

import { AbstractFormState, FormControlValidators } from '@bcgov/shared/ui';

import { AdministratorInformation } from './administrator-information.model';

export class AdministratorInformationFormState extends AbstractFormState<AdministratorInformation> {
  public constructor(private fb: FormBuilder) {
    super();

    this.buildForm();
  }

  public get json(): AdministratorInformation | undefined {
    if (!this.formInstance) {
      return;
    }

    return this.formInstance.getRawValue();
  }

  public patchValue(model: AdministratorInformation | null): void {
    if (!this.formInstance || !model) {
      return;
    }

    this.formInstance.patchValue(model);
  }

  public buildForm(): void {
    this.formInstance = this.fb.group({
      email: [null, [Validators.required, FormControlValidators.email]],
    });
  }
}
