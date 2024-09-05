import { FormBuilder, FormControl, Validators } from '@angular/forms';

import { AbstractFormState } from '@bcgov/shared/ui';

import { PartyLicenceDeclarationInformation } from './party-licence-declaration-information.model';

export class CollegeLicenceDeclarationFormState extends AbstractFormState<PartyLicenceDeclarationInformation> {
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

    const values = this.formInstance.getRawValue();

    // Map '0' in the form back into null for the server (mat-select can't use null as a value)
    if (values.collegeCode === 0) {
      values.collegeCode = null;
      values.licenceNumber = null;
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
      collegeCode: [{ disabled: false, value:0 }, [Validators.required]],
      licenceNumber: [{ disabled: false, value: '' }],
    });

    this.collegeCode.valueChanges.subscribe((value) =>
      this.onCollegeCodeValueChanged(value),
    );
  }

  public disableCollegeLicenseForm(): void {
    this.collegeCode.disable();
    this.licenceNumber.disable();
  }

  private onCollegeCodeValueChanged(collegeCode: number | null): void {
    if (collegeCode) {
      this.licenceNumber.setValidators([Validators.required]);
    } else {
      this.licenceNumber.clearValidators();
    }

    this.licenceNumber.updateValueAndValidity();
  }
}
