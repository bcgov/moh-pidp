import {
  AbstractControl,
  FormBuilder,
  ValidationErrors,
  Validators,
} from '@angular/forms';

import { AbstractFormState } from '@bcgov/shared/ui';

import { EndorsementRequestInformation } from './models/endorsement-request.model';

// TODO: Change to allow trailing spaces, then trim them later!
export const EmailRegexPattern =
  /^[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\s*$/;
export class MultilineEmailValidator {
  private static emailRegex = new RegExp(EmailRegexPattern);

  public static multilineEmail(
    control: AbstractControl
  ): ValidationErrors | null {
    const value = control.value ? String(control.value) : '';
    if (!value) {
      return null;
    }
    return MultilineEmailValidator.validateMultilineEmail(value);
  }
  public static validateMultilineEmail(value: string): ValidationErrors | null {
    if (!value) return null;

    // Split the multiline text value into individual lines of text.
    const lines = value.split('\n');

    // Validate each line individually.
    let errorCount = 0;
    lines.forEach((line: string) => {
      const isValid = this.validateEmail(line);
      if (!isValid) {
        errorCount++;
      }
    });

    if (errorCount > 0) {
      return { multilineEmail: true };
    }
    return null;
  }

  public static validateEmail(value: string): boolean {
    if (value) {
      const isValid = this.emailRegex.test(value);
      return isValid;
    }

    // Empty strings are considered valid.
    return true;
  }
}
export class EndorsementsFormState extends AbstractFormState<EndorsementRequestInformation> {
  public constructor(private fb: FormBuilder) {
    super();

    this.buildForm();
  }

  public get json(): EndorsementRequestInformation | undefined {
    if (!this.formInstance) {
      return;
    }

    return this.formInstance.getRawValue();
  }

  public patchValue(model: EndorsementRequestInformation | null): void {
    if (!this.formInstance || !model) {
      return;
    }

    this.formInstance.patchValue(model);
  }

  public buildForm(): void {
    this.formInstance = this.fb.group({
      recipientEmails: [
        null,
        [Validators.required, MultilineEmailValidator.multilineEmail],
      ],
    });
  }
}
