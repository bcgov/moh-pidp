import { Component, TemplateRef, ViewChild, ViewEncapsulation } from '@angular/core';
import { MatDialog, MatDialogConfig, MatDialogRef } from '@angular/material/dialog';
import {  MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { FeedbackFormState } from './feedback-form.component-form-state';
import { AbstractFormDependenciesService, AbstractFormPage } from '@app/core/classes/abstract-form-page.class';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { Observable } from 'rxjs';
import { SuccessDialogComponent } from "../success-dialog/success-dialog.component";
import { MatInputModule } from '@angular/material/input';
import { NgIf } from '@angular/common';
import html2canvas from 'html2canvas';
import { FeedbackSendComponent } from '../success-dialog/components/feedback-send.component';
import { faXmark } from '@fortawesome/free-solid-svg-icons';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-feedback-form-dialog',
  templateUrl: './feedback-form.dialog.html',
  styleUrl: './feedback-form.dialog.scss',
  standalone: true,
  imports: [
    SuccessDialogComponent,
    MatFormFieldModule,
    MatInputModule,
    NgIf,
    ReactiveFormsModule,
    MatIconModule,
    MatButtonModule
  ],
  encapsulation: ViewEncapsulation.None,
})
export class FeedbackFormDialogComponent
  extends AbstractFormPage<FeedbackFormState>
{
  public formState: FeedbackFormState;
  public showOverlayOnSubmit: boolean = false;
  public showErrorCard: boolean = false;
  public selectedFile: any = null;
  public componentType = FeedbackSendComponent;
  public faXmark = faXmark;

  @ViewChild('successDialog')
  public successDialogTemplate!: TemplateRef<any>;

  public constructor(
    dependenciesService: AbstractFormDependenciesService,
    fb: FormBuilder,
    dialog: MatDialog,
    public dialogRef: MatDialogRef<FeedbackFormDialogComponent>,

  ) {
    super(dependenciesService);
    this.formState = new FeedbackFormState(fb, dialog);
  }


  protected performSubmission(): Observable<unknown> {
    return new Observable;
  }

  public onFileSelected(): void {
    const inputNode: any = document.querySelector('#file');
    this.selectedFile = inputNode.files[0] ?? null;
  }

  public takeScreenshot(event: Event): void {
    event.preventDefault();
    const element = document.getElementById('app');
    if (element) {
      html2canvas(element).then(canvas => {
        const imgData = canvas.toDataURL('image/png');
        const link = document.createElement('a');
        link.href = imgData;
        link.download = 'screenshot.png';
        link.click();
      });
    }
  }

  public uploadFile(event: Event): void {
    event.preventDefault();
    const element = document.getElementById("file");
    element?.click();
  }

  public onCloseClick(): void {
    this.dialogRef.close();
  }

  public showSuccessDialog(event: Event): void {
    event.preventDefault();
    if(this.formUtilsService.checkValidity(this.formState.form)) {
      this.dialogRef.close();
      const config: MatDialogConfig = {
        disableClose: true,
        position: {
          right: "0px"
        },
        width: '380px',
      };
      this.dialog.open(this.successDialogTemplate, config);
    }
  }

}
