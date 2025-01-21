import { Component, OnInit, TemplateRef, ViewChild, ViewEncapsulation } from '@angular/core';
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
import { FeedbackFormDialogResource } from './feedback-form-dialog-resource.service';
import { FeedbackSuccessResponse } from './feedback-form-dialog-success.response.model';
import { HttpErrorResponse } from '@angular/common/http';
import { LoggerService } from '@app/core/services/logger.service';
import { PartyService } from '@app/core/party/party.service';
import { ProfileStatus } from '@app/features/portal/models/profile-status.model';
import { PortalResource } from '@app/features/portal/portal-resource.service';
import { PersonalInformation } from '@app/features/profile/pages/personal-information/personal-information.model';

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
  extends AbstractFormPage<FeedbackFormState> implements OnInit
{
  public formState: FeedbackFormState;
  public showOverlayOnSubmit: boolean = false;
  public showErrorCard: boolean = false;
  public selectedFile: File | null = null;
  public componentType = FeedbackSendComponent;
  public faXmark = faXmark;
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
    private feedbackFormDialogResource: FeedbackFormDialogResource,
    private logger: LoggerService,
    private partyService: PartyService,
    private portalResource: PortalResource,
    private resource: FeedbackFormDialogResource,
  ) {
    super(dependenciesService);
    this.formState = new FeedbackFormState(fb, dialog);
  }


  protected performSubmission(): Observable<unknown> {
    return new Observable;
  }

  public onFileSelected(): void {
    const inputNode: HTMLInputElement | null = document.querySelector('#file');
    this.selectedFile= inputNode?.files ? inputNode.files[0] : null;
  }

  public takeScreenshot(event: Event): void {
    event.preventDefault();
    const element = document.getElementById('app');
    if (element) {
      html2canvas(element).then((canvas: HTMLCanvasElement) => {
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

  public onCloseClick(event: Event): void {
    event.preventDefault();
    this.dialogRef.close();
  }

  public sendFeedback(event: Event): void {
    event.preventDefault();
    const formData = new FormData();
    if(this.selectedFile) {
      formData.append('file', this.selectedFile, this.selectedFile?.name);
    }
    formData.append('feedback', this.formState.feedback.value);
    formData.append('partyid', this.partyService.partyId.toString());

    this.feedbackFormDialogResource.postFeedback(formData).subscribe(
      (data: FeedbackSuccessResponse) => {
        this.showSuccessDialog();
      },
      (err: HttpErrorResponse) => {
        this.logger.error(
          '[FeedbackFormDialogResource::postFeedback] error has occurred: ',
          err,
        );
      },
    );
  }

  public showSuccessDialog(): void {
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

  private getProfileStatus(partyId: number): Observable<ProfileStatus | null> {
    return this.portalResource.getProfileStatus(partyId);
  }
}
