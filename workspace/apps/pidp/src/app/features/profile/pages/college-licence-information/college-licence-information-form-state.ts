import { FormBuilder, FormControl, Validators } from '@angular/forms';

import { AbstractFormState, FormControlValidators } from '@bcgov/shared/ui';

import { PartyLicenceDeclarationInformation } from './party-licence-declaration-information.model';

export class CollegeLicenceInformationFormState extends AbstractFormState<PartyLicenceDeclarationInformation> {
  public constructor(private fb: FormBuilder) {
    super();

    this.buildForm();
  }

  public get collegeCode(): FormControl {
    return this.formInstance.get('collegeCode') as FormControl;
  }

  public get licenceNumber(): FormControl {
    return this.formInstance.get('licenceNumber') as FormControl;
  }

  public get json(): PartyLicenceDeclarationInformation | undefined {
    if (!this.formInstance) {
      return;
    }

    const values =
      this.formInstance.getRawValue() as PartyLicenceDeclarationInformation;

    // Map '0' in the form back into null for the server (mat-select can't use null as a value)
    if (values.collegeCode === 0) {
      values.collegeCode = null;
    }

    return values;
  }

  public patchValue(model: PartyLicenceDeclarationInformation | null): void {
    if (!this.formInstance || !model) {
      return;
    }

    // Map null from server into '0' in the form (mat-select can't use null as a value)
    if (model.collegeCode === null) {
      model.collegeCode = 0;
    }

    this.formInstance.patchValue(model);
  }

  public buildForm(): void {
    this.formInstance = this.fb.group({
      collegeCode: [
        0,
        [Validators.required, FormControlValidators.requiredIndex],
      ],
      licenceNumber: ['', [Validators.required]],
    });
  }
}
