import { FormBuilder, FormControl, Validators } from '@angular/forms';

import { AbstractFormState } from '@bcgov/shared/ui';

import { MsTeamsClinicInfo } from './ms-teams.model';

export class MsTeamsFormState extends AbstractFormState<MsTeamsClinicInfo> {
  public constructor(private fb: FormBuilder) {
    super();

    this.buildForm();
  }

  // public get ldapUsername(): FormControl {
  //   return this.formInstance.get('ldapUsername') as FormControl;
  // }

  // public get ldapPassword(): FormControl {
  //   return this.formInstance.get('ldapPassword') as FormControl;
  // }

  public get json(): MsTeamsClinicInfo | undefined {
    if (!this.formInstance) {
      return;
    }

    return this.formInstance.getRawValue();
  }

  public patchValue(model: MsTeamsClinicInfo | null): void {
    if (!this.formInstance || !model) {
      return;
    }

    this.formInstance.patchValue(model);
  }

  public buildForm(): void {
    this.formInstance = this.fb.group({});
  }
}
