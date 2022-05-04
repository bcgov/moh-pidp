import { FormBuilder } from '@angular/forms';

import { AbstractFormState, FormControlValidators } from '@bcgov/shared/ui';

import { HcimEnrolment } from './hcim-enrolment.model';

export class HcimEnrolmentFormState extends AbstractFormState<HcimEnrolment> {
  public constructor(private fb: FormBuilder) {
    super();

    this.buildForm();
  }

  public get json(): HcimEnrolment | undefined {
    if (!this.formInstance) {
      return;
    }

    return this.formInstance.getRawValue();
  }

  public patchValue(): void {
    // Form will never be patched!
    throw new Error('Method Not Implemented');
  }

  public buildForm(): void {
    this.formInstance = this.fb.group({
      managesTasks: [null, [FormControlValidators.requiredBoolean]],
      modifiesPhns: [null, [FormControlValidators.requiredBoolean]],
      recordsNewborns: [null, [FormControlValidators.requiredBoolean]],
      searchesIdentifiers: [null, [FormControlValidators.requiredBoolean]],
    });
  }
}
