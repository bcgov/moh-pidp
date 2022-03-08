import { FormBuilder, FormControl, Validators } from '@angular/forms';

import { AbstractFormState } from '@bcgov/shared/ui';

import { HcimReenrolmentModel } from './hcim-reenrolment.model';

export class HcimReenrolmentFormState extends AbstractFormState<HcimReenrolmentModel> {
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

  public get json(): HcimReenrolmentModel | undefined {
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
      ldapUsername: [null, [Validators.required]],
      ldapPassword: [null, [Validators.required]],
    });
  }
}
