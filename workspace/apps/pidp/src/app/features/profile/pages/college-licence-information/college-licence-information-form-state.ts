import { FormBuilder, FormControl, Validators } from '@angular/forms';

import { AbstractFormState } from '@bcgov/shared/ui';

import { CollegeLicenceInformationModel } from './college-licence-information.model';

export class CollegeLicenceInformationFormState extends AbstractFormState<CollegeLicenceInformationModel> {
  public constructor(private fb: FormBuilder) {
    super();

    this.buildForm();
  }

  public get collegeCode(): FormControl {
    return this.formInstance.get('collegeCode') as FormControl;
  }

  public get collegeLicence(): FormControl {
    return this.formInstance.get('collegeLicence') as FormControl;
  }

  public get json(): CollegeLicenceInformationModel | undefined {
    if (!this.formInstance) {
      return;
    }

    return this.formInstance.getRawValue();
  }

  public patchValue(model: CollegeLicenceInformationModel | null): void {
    if (!this.formInstance || !model) {
      return;
    }

    this.formInstance.patchValue(model);
  }

  public buildForm(): void {
    this.formInstance = this.fb.group({
      collegeCode: ['', [Validators.required]],
      collegeLicence: ['', []],
    });
  }
}
