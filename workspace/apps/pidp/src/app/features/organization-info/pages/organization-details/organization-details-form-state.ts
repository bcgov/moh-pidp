import { FormBuilder, FormControl, Validators } from '@angular/forms';

import { AbstractFormState } from '@bcgov/shared/ui';

import { OrganizationDetails } from './organization-details.model';

export class OrganizationDetailsFormState extends AbstractFormState<OrganizationDetails> {
  public constructor(private fb: FormBuilder) {
    super();

    this.buildForm();
  }

  public get organizationType(): FormControl {
    return this.formInstance.get('organizationType') as FormControl;
  }

  public get json(): OrganizationDetails | undefined {
    if (!this.formInstance) {
      return;
    }

    return this.formInstance.getRawValue();
  }

  public patchValue(model: OrganizationDetails | null): void {
    if (!this.formInstance || !model) {
      return;
    }

    this.formInstance.patchValue(model);
  }

  public buildForm(): void {
    this.formInstance = this.fb.group({
      organizationType: [null, [Validators.required]],
      healthAuthorityType: [null, [Validators.required]],
      employeeId: [null, [Validators.required]],
    });
  }
}
