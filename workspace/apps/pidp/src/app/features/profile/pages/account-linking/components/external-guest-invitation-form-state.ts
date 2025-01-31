import { FormBuilder, FormControl, Validators } from '@angular/forms';

import { AbstractFormState } from '@bcgov/shared/ui';

import { ExternalGuestInvitationInformation } from './external-guest-invitation-information-model';
import { ExternalGuestInvitation } from '@bcgov/shared/data-access';

export class ExternalGuestInvitationFormState extends AbstractFormState<ExternalGuestInvitationInformation> {
  public constructor(private fb: FormBuilder) {
    super();

    this.buildForm();
  }

  public get username(): FormControl {
    return this.formInstance.get('username') as FormControl;
  }

  public get json(): ExternalGuestInvitation | undefined {
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

  public patchValue(model: ExternalGuestInvitation | null): void {
    if (!this.formInstance || !model) {
      return;
    }

    // Map null from server into '' in the form (mat-select can't use null as a value)
    if (model.username === null) {
      model.username = "";
    }

    this.formInstance.patchValue(model);
  }

  public buildForm(): void {
    this.formInstance = this.fb.group({
      username: [{ disabled: false, value: '' }, [Validators.required]],
    });

    this.username.valueChanges.subscribe((value) =>
      this.onUserNameChanged(value),
    );
  }

  public disableExternalGuestInvitationForm(): void {
    this.username.disable();
  }

  private onUserNameChanged(username: string | null): void {
    if (username){
      this.username.setValidators([Validators.required]);
    } else {
      this.username.clearValidators();
    }

    this.username.updateValueAndValidity();
  }
}
