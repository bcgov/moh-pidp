import { FormBuilder, FormControl, Validators } from '@angular/forms';

import { AbstractFormState } from '@bcgov/shared/ui';

import { HcimEnrolment } from './hcim-enrolment.model';

export class HcimEnrolmentFormState extends AbstractFormState<HcimEnrolment> {
  public constructor(private fb: FormBuilder) {
    super();

    this.buildForm();
  }

  public get adminEmail(): FormControl {
    return this.formInstance.get('adminEmail') as FormControl;
  }

  public get json(): HcimEnrolment | undefined {
    if (!this.formInstance) {
      return;
    }

    return this.formInstance.getRawValue();
  }

  public patchValue(): void {}

  public buildForm(): void {
    this.formInstance = this.fb.group({
      adminEmail: [null, [Validators.required]],
    });
  }
}
