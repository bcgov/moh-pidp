import { Component, Inject, OnDestroy, OnInit, TemplateRef, ViewChild, ViewEncapsulation } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogConfig, MatDialogRef } from '@angular/material/dialog';
import {  MatFormFieldModule } from '@angular/material/form-field';
import { FeedbackFormState } from './feedback-button.component-form-state';
import { AbstractFormDependenciesService, AbstractFormPage } from '@app/core/classes/abstract-form-page.class';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { Observable } from 'rxjs';
import { SuccessDialogComponent } from "../success-dialog/success-dialog.component";
import { MatInputModule } from '@angular/material/input';
import { NgIf } from '@angular/common';

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
    ReactiveFormsModule
  ],
  encapsulation: ViewEncapsulation.None,
})
export class FeedbackFormDialogComponent
  extends AbstractFormPage<FeedbackFormState>
  implements OnInit, OnDestroy
{
  public formState: FeedbackFormState;
  public showOverlayOnSubmit: boolean = false;
  public showErrorCard: boolean = false;
  public selectedFile: any = null;

  @ViewChild('successDialog')
  public successDialogTemplate!: TemplateRef<any>;

  constructor(
    dependenciesService: AbstractFormDependenciesService,
    fb: FormBuilder,
    dialog: MatDialog,
    public dialogRef: MatDialogRef<FeedbackFormDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    super(dependenciesService);
    this.formState = new FeedbackFormState(fb, dialog);
  }

  ngOnInit(): void {

  }

  ngOnDestroy(): void {

  }

  protected performSubmission(): Observable<unknown> {
    return new Observable;
  }

  public onFileSelected(): void {
    const inputNode: any = document.querySelector('#file');
    this.selectedFile = inputNode.files[0] ?? null;
    console.log("selectedFile : ", this.selectedFile);
  }

  public onNoClick(): void {
    this.dialogRef.close();
  }
  onClose(): void {
    this.dialogRef.close('Closed by user');
  }

  private showSuccessDialog(): void {
    const config: MatDialogConfig = {
      disableClose: true,
    };
    this.dialog.open(this.successDialogTemplate, config);
  }
}
