import { CommonModule } from '@angular/common';
import { Component, TemplateRef, ViewChild } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { SuccessDialogComponent } from "../success-dialog/success-dialog.component";
import { FeedbackFormDialogComponent } from '../feedback-form/feedback-form.dialog';
import { MatDialog } from '@angular/material/dialog';
import { right } from '@popperjs/core';

@Component({
  selector: 'app-feedback-button',
  templateUrl: './feedback-button.component.html',
  styleUrl: './feedback-button.component.scss',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    SuccessDialogComponent
],
})
export class FeedbackButtonComponent
{

  public isFeedbackFormOpen: boolean = false;
  public showOverlayOnSubmit: boolean = false;
  public showErrorCard: boolean = false;

  @ViewChild('successDialog')
  public successDialogTemplate!: TemplateRef<any>;

  public constructor(public dialog: MatDialog)
  {
  }

  public toggleFeedbackForm(): void {
    this.isFeedbackFormOpen = !this.isFeedbackFormOpen;
  }

  public openDialog(): void {
    const dialogRef = this.dialog.open(FeedbackFormDialogComponent, {
      position: {
        right: "0px"
      },
      width: '380px',
      data: { title: 'Hello!', content: 'This is a modal dialog.' },
      panelClass: 'custom-dialog-container'
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      console.log('Result:', result);
    });
  }

}
