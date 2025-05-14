import {
  FormBuilder,
  FormControl,
  Validators,
} from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';

import { AbstractFormState } from '@bcgov/shared/ui';

export interface FeedbackFormData {
  feedback: string;
  attachment: File;
}

export class FeedbackFormState extends AbstractFormState<FeedbackFormData> {
  public constructor(
    private readonly fb: FormBuilder,
    public dialog: MatDialog
  ) {
    super();

    this.buildForm();
  }

  public get feedback(): FormControl {
    return this.formInstance.get('feedback') as FormControl;
  }

  public get attachment(): FormControl {
    return this.formInstance.get('attachment') as FormControl;
  }

  public get json(): FeedbackFormData | undefined {
    if (!this.formInstance) {
      return undefined;
    }

    const values = this.formInstance.getRawValue();

    return values;
  }

  public patchValue(model: FeedbackFormData | null): void {
    if (!this.formInstance || !model) {
      return;
    }

    this.formInstance.patchValue(model);
  }

  public buildForm(): void {
    this.formInstance = this.fb.group(
      {
        feedback: ['', [Validators.required]],
        attachment: [''],
      }
    );
  }

  public getErrorMessage(): string {
    const errors = this.feedback.errors;
    if (errors) {
      if (errors.required) {
        return 'Required';
      }
    }
    return '';
  }

}
