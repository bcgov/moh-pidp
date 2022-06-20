import { FormBuilder, Validators } from '@angular/forms';

import { AbstractFormState, FormControlValidators } from '@bcgov/shared/ui';

import { EndorsementRequest } from './endorsement-request.model';

export class EndorsementRequestFormState extends AbstractFormState<EndorsementRequest> {
  public constructor(private fb: FormBuilder) {
    super();

    this.buildForm();
  }

  public get json(): EndorsementRequest | undefined {
    if (!this.formInstance) {
      return;
    }

    return this.formInstance.getRawValue();
  }

  public patchValue(model: EndorsementRequest | null): void {
    if (!this.formInstance || !model) {
      return;
    }

    this.formInstance.patchValue(model);
  }

  public buildForm(): void {
    this.formInstance = this.fb.group({
      recipientEmail: [
        null,
        [Validators.required, FormControlValidators.email],
      ],
      jobTitle: [null, Validators.required],
    });
  }
}
