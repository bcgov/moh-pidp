import { FormBuilder, Validators } from '@angular/forms';

import { AbstractFormState, FormControlValidators } from '@bcgov/shared/ui';

import { Endorsement } from './endorsement.model';

export class EndorsementFormState extends AbstractFormState<Endorsement> {
  public constructor(private fb: FormBuilder) {
    super();

    this.buildForm();
  }

  public get json(): Endorsement | undefined {
    if (!this.formInstance) {
      return;
    }

    return this.formInstance.getRawValue();
  }

  public patchValue(model: Endorsement | null): void {
    if (!this.formInstance || !model) {
      return;
    }

    this.formInstance.patchValue(model);
  }

  public buildForm(): void {
    this.formInstance = this.fb.group({
      recipientEmail: [null, [Validators.required, FormControlValidators.email]],
    });
  }
}
