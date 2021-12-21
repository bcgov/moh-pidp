import { FormBuilder, FormGroup } from '@angular/forms';

import { AbstractFormState } from '@bcgov/shared/ui';

import { WorkAndRoleInformationModel } from './work-and-role-information.model';

export class WorkAndRoleInformationFormState extends AbstractFormState<WorkAndRoleInformationModel> {
  public constructor(private fb: FormBuilder) {
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
      physicalAddress: this.fb.group({
        countryCode: [{ value: null, disabled: false }, []],
        provinceCode: [{ value: null, disabled: false }, []],
        street: [{ value: null, disabled: false }, []],
        city: [{ value: null, disabled: false }, []],
        postal: [{ value: null, disabled: false }, []],
      }),
    });
  }
}
