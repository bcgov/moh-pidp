import { FormBuilder } from '@angular/forms';

import { AbstractFormState } from '@bcgov/shared/ui';

import { FormUtilsService } from '@app/core/services/form-utils.service';

import { ClinicId } from './ms-teams-clinic-member.model';

export class MsTeamsClinicMemberFormState extends AbstractFormState<ClinicId> {
  public constructor(
    private fb: FormBuilder,
    private formUtilsService: FormUtilsService,
  ) {
    super();

    this.buildForm();
  }

  public get json(): ClinicId | undefined {
    if (!this.formInstance) {
      return undefined;
    }

    return this.formInstance.getRawValue();
  }

  public patchValue(model: ClinicId | null): void {
    if (!this.formInstance || !model) {
      return;
    }

    this.formInstance.patchValue(model);
  }

  public buildForm(): void {
    this.formInstance = this.fb.group({
      clinicId: [null, []],
    });
  }
}
