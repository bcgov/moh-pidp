import { FormBuilder, FormControl, Validators } from '@angular/forms';

import { AbstractFormState, FormControlValidators } from '@bcgov/shared/ui';

import { OrganizationDetails } from './organization-details.model';

export class OrganizationDetailsFormState extends AbstractFormState<OrganizationDetails> {
  public constructor(private fb: FormBuilder) {
    super();

    this.buildForm();
  }

  public get organizationCode(): FormControl {
    return this.formInstance.get('organizationCode') as FormControl;
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
      organizationCode: [0, [Validators.required, FormControlValidators.requiredIndex]],
      healthAuthorityCode: [0, [Validators.required, FormControlValidators.requiredIndex]],
      employeeIdentifier: ['', [Validators.required]],
    });
  }
}
