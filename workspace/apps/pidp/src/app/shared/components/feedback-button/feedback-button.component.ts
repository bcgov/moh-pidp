import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FeedbackFormDialogComponent } from '../feedback-form/feedback-form.dialog';
import { MatDialog } from '@angular/material/dialog';
import { PidpViewport, ViewportService } from '@bcgov/shared/ui';

@Component({
    selector: 'app-feedback-button',
    templateUrl: './feedback-button.component.html',
    styleUrl: './feedback-button.component.scss',
    imports: [
        CommonModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule
    ]
})
export class FeedbackButtonComponent
{

  public dialogWidth: string = "360px";
  public isMobileView: boolean = false;
  public buttonClass: string = "feedback-button-box";

  public constructor(
    public dialog: MatDialog,
    viewportService: ViewportService
  )
  {
    viewportService.viewportBroadcast$.subscribe((viewport) =>
      this.onViewportChange(viewport),
    );
  }

  public openDialog(): void {
    this.dialog.open(FeedbackFormDialogComponent, {
      position: {
        right: "0px"
      },
      width: this.dialogWidth,
    });

  }

  private onViewportChange(viewport: PidpViewport): void {
    if (viewport === PidpViewport.xsmall) {
      this.dialogWidth = "315px";
      this.isMobileView = true;
      this.buttonClass = "feedback-button-box-mobile";
    } else {
      this.dialogWidth ="360px";
      this.isMobileView =false;
      this.buttonClass = "feedback-button-box";
    }
  }

}
