import { FormBuilder, FormControl, Validators } from '@angular/forms';

import { AbstractFormState } from '@bcgov/shared/ui';

import { HcimWebEnrolmentModel } from './hcim-web-enrolment.model';

export class HcimWebEnrolmentFormState extends AbstractFormState<HcimWebEnrolmentModel> {
  public constructor(private fb: FormBuilder) {
    super();

    this.buildForm();
  }

  public get ldapUsername(): FormControl {
    return this.formInstance.get('ldapUsername') as FormControl;
  }

  public get ldapPassword(): FormControl {
    return this.formInstance.get('ldapPassword') as FormControl;
  }

  public get json(): HcimWebEnrolmentModel | undefined {
    if (!this.formInstance) {
      return;
    }

    return this.formInstance.getRawValue();
  }

  public patchValue(model: HcimWebEnrolmentModel | null): void {
    if (!this.formInstance || !model) {
      return;
    }

    // TODO unless more is going into the form it will never be patched
    // this.formInstance.patchValue(model);
  }

  public buildForm(): void {
    this.formInstance = this.fb.group({
      ldapUsername: [null, [Validators.required]],
      ldapPassword: [null, [Validators.required]],
    });
  }
}
