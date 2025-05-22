import { FormBuilder, Validators } from '@angular/forms';

import { AbstractFormState, FormControlValidators } from '@bcgov/shared/ui';

import { EndorsementRequestInformation } from './models/endorsement-request-information.model';

export class EndorsementsFormState extends AbstractFormState<EndorsementRequestInformation> {
  public constructor(private readonly fb: FormBuilder) {
    super();

    this.buildForm();
  }

  public get json(): EndorsementRequestInformation | undefined {
    if (!this.formInstance) {
      return undefined;
    }

    return this.formInstance.getRawValue();
  }

  public patchValue(model: EndorsementRequestInformation | null): void {
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
    });
  }
}
