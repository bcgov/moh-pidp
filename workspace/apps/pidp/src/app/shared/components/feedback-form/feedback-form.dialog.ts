import { NgIf } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import {
  MatDialog,
  MatDialogConfig,
  MatDialogRef,
} from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';

import { Observable } from 'rxjs';

import { faXmark } from '@fortawesome/free-solid-svg-icons';
import html2canvas from 'html2canvas';

import {
  AbstractFormDependenciesService,
  AbstractFormPage,
} from '@app/core/classes/abstract-form-page.class';
import { PartyService } from '@app/core/party/party.service';
import { LoggerService } from '@app/core/services/logger.service';
import { ToastService } from '@app/core/services/toast.service';
import { ProfileRoutes } from '@app/features/profile/profile.routes';

import { FeedbackSendComponent } from '../success-dialog/components/feedback-send.component';
import { SuccessDialogComponent } from '../success-dialog/success-dialog.component';
import { FeedbackFormDialogResource } from './feedback-form-dialog-resource.service';
import { FeedbackFormState } from './feedback-form.component-form-state';

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
    MatButtonModule,
  ],
})
export class FeedbackFormDialogComponent
  extends AbstractFormPage<FeedbackFormState>
  implements OnInit
{
  public formState: FeedbackFormState;
  public showOverlayOnSubmit: boolean = false;
  public showErrorCard: boolean = false;
  public selectedFile: File | null = null;
  public componentType = FeedbackSendComponent;
  public faXmark = faXmark;
  public disableSend: boolean = true;
  public disableDisclaimer: boolean = true;
  public accessAgreementLink: string = ProfileRoutes.routePath(
    ProfileRoutes.USER_ACCESS_AGREEMENT,
  );
  public disclaimerText: string = `Please do not include any personal information when submitting this feedback. For additional information, please refer to <a href=${this.accessAgreementLink} target="_blank">this page</a>.`;

  @ViewChild('successDialog')
  public successDialogTemplate!: TemplateRef<FeedbackSendComponent>;

  public ngOnInit(): void {
    const textarea = document.getElementById('auto-expand');

    textarea?.addEventListener('input', () => {
      textarea.style.height = 'auto';
      textarea.style.height = `${Math.min(textarea.scrollHeight, 200)}px`;
    });
  }
  public constructor(
    dependenciesService: AbstractFormDependenciesService,
    fb: FormBuilder,
    dialog: MatDialog,
    public dialogRef: MatDialogRef<FeedbackFormDialogComponent>,
    private readonly feedbackFormDialogResource: FeedbackFormDialogResource,
    private readonly logger: LoggerService,
    private readonly partyService: PartyService,
    private readonly toastService: ToastService,
  ) {
    super(dependenciesService);
    this.formState = new FeedbackFormState(fb, dialog);
  }

  protected performSubmission(): Observable<unknown> {
    return new Observable();
  }

  public onFeedbackChange(event: Event): void {
    event.preventDefault();
    this.disableSend = this.disableDisclaimer =
      this.formState.feedback.value.length === 0;
  }

  public onFileSelected(): void {
    const inputNode: HTMLInputElement | null = document.querySelector('#file');
    this.selectedFile = inputNode?.files ? inputNode.files[0] : null;
    if ((this.selectedFile?.size ?? 0) > 5 * 1024 * 1024) {
      this.showErrorCard = true;
    } else {
      this.showErrorCard = false;
    }
  }

  public takeScreenshot(event: Event): void {
    event.preventDefault();
    const element = document.getElementById('app');
    const header = document.querySelector('header');
    if (element) {
      if (header) header.style.position = 'relative';
      html2canvas(element).then((canvas: HTMLCanvasElement) => {
        if (header) header.style.position = 'sticky';
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
    const element = document.getElementById('file');
    element?.click();
  }

  public onCloseClick(event: Event): void {
    event.preventDefault();
    this.dialogRef.close();
  }

  public sendFeedback(event: Event): void {
    event.preventDefault();
    if (this.showErrorCard) {
      return;
    }
    const formData = new FormData();
    if (this.selectedFile) {
      formData.append('file', this.selectedFile, this.selectedFile?.name);
    }
    formData.append('feedback', this.formState.feedback.value);
    formData.append('partyid', this.partyService.partyId.toString());

    this.feedbackFormDialogResource.postFeedback(formData).subscribe(
      () => {
        this.showSuccessDialog();
      },
      (err: HttpErrorResponse) => {
        this.logger.error(
          '[FeedbackFormDialogResource::postFeedback] error has occurred: ',
          err,
        );
        this.toastService.openErrorToast(
          'Error occurred while sending feedback',
        );
      },
    );
  }

  public showSuccessDialog(): void {
    if (this.formUtilsService.checkValidity(this.formState.form)) {
      this.dialogRef.close();
      const config: MatDialogConfig = {
        disableClose: true,
        position: {
          right: '0px',
        },
        width: '380px',
      };
      this.dialog.open(this.successDialogTemplate, config);
    }
  }
}
