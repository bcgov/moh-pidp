import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { AbstractFormState } from '@bcgov/shared/ui';

import { FormUtilsService } from '@app/core/services/form-utils.service';

import { WorkAndRoleInformationModel } from './work-and-role-information.model';

export class WorkAndRoleInformationFormState extends AbstractFormState<WorkAndRoleInformationModel> {
  public constructor(
    private fb: FormBuilder,
    private formUtilsService: FormUtilsService
  ) {
    super();

    this.buildForm();
  }

  public get physicalAddress(): FormGroup {
    return this.form.get('physicalAddress') as FormGroup;
  }

  public get json(): WorkAndRoleInformationModel | undefined {
    if (!this.formInstance) {
      return;
    }

    return this.formInstance.getRawValue();
  }

  public patchValue(model: WorkAndRoleInformationModel | null): void {
    if (!this.formInstance || !model) {
      return;
    }

    this.formInstance.patchValue(model);
  }

  public buildForm(): void {
    this.formInstance = this.fb.group({
      jobTitle: ['', [Validators.required]],
      facilityName: ['', [Validators.required]],
      physicalAddress: this.formUtilsService.buildAddressForm({
        areRequired: true,
      }),
    });
  }
}
