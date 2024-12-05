import {
  FormBuilder,
  FormControl,
  Validators,
} from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';

import { AbstractFormState } from '@bcgov/shared/ui';
import { FeedbackFormDialogComponent } from '../feedback-form/feedback-form.dialog';

export interface FeedbackFormData {
  feedback: string;
  attachment: File;
}

export class FeedbackFormState extends AbstractFormState<FeedbackFormData> {
  public constructor(
    private fb: FormBuilder,
    public dialog: MatDialog
  ) {
    super();

    this.buildForm();
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(FeedbackFormDialogComponent, {
      width: '300px',
      data: { title: 'Hello!', content: 'This is a modal dialog.' },
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      console.log('Result:', result);
    });
  }

  public get feedback(): FormControl {
    return this.formInstance.get('feedback') as FormControl;
  }

  public get attachment(): FormControl {
    return this.formInstance.get('attachment') as FormControl;
  }

  public get json(): FeedbackFormData | undefined {
    if (!this.formInstance) {
      return;
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
        attachment: ['', [Validators.required]],
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
