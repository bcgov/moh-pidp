import { CommonModule } from '@angular/common';
import { Component, TemplateRef, ViewChild } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { SuccessDialogComponent } from "../success-dialog/success-dialog.component";
import { FeedbackFormDialogComponent } from '../feedback-form/feedback-form.dialog';
import { MatDialog } from '@angular/material/dialog';
import { PidpViewport, ViewportService } from '@bcgov/shared/ui';

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
  public dialogWidth: string = "360px";

  @ViewChild('successDialog')
  public successDialogTemplate!: TemplateRef<any>;

  public constructor(
    public dialog: MatDialog,
    viewportService: ViewportService
  )
  {
    viewportService.viewportBroadcast$.subscribe((viewport) =>
      this.onViewportChange(viewport),
    );
  }

  public toggleFeedbackForm(): void {
    this.isFeedbackFormOpen = !this.isFeedbackFormOpen;
  }

  public openDialog(): void {
    const dialogRef = this.dialog.open(FeedbackFormDialogComponent, {
      position: {
        right: "0px"
      },
      width: this.dialogWidth,
    });

    dialogRef.afterClosed().subscribe(result => {
      //TO-DO: implement the sending feedback to backend.
    });
  }

  private onViewportChange(viewport: PidpViewport): void {
    if (viewport === PidpViewport.xsmall) {
      this.dialogWidth = "290px";
    } else {
      this.dialogWidth ="360px";
    }
  }

}
