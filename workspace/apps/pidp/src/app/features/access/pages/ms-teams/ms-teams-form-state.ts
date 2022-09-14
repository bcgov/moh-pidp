import { FormBuilder, FormControl, Validators } from '@angular/forms';

import { AbstractFormState } from '@bcgov/shared/ui';

import { FormUtilsService } from '@app/core/services/form-utils.service';

import { MsTeamsClinicInfo } from './ms-teams.model';

export class MsTeamsFormState extends AbstractFormState<MsTeamsClinicInfo> {
  public constructor(
    private fb: FormBuilder,
    private formUtilsService: FormUtilsService
  ) {
    super();

    this.buildForm();
  }

  public get clinicName(): FormControl {
    return this.formInstance.get('clinicName') as FormControl;
  }

  public get clinicAddress(): FormControl {
    return this.formInstance.get('clinicAddress') as FormControl;
  }

  public get clinicMembers(): FormControl {
    return this.formInstance.get('clinicMembers') as FormControl;
  }

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
    this.formInstance = this.fb.group({
      clinicName: [null, [Validators.required]],
      clinicAddress: this.formUtilsService.buildAddressForm({
        areRequired: true,
      }),
      clinicMembers: [[], [Validators.required]],
    });
  }
}
