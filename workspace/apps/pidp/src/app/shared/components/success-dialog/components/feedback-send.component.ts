import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

import { faXmark } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-dialog-feedback-send',
  template: ` <div style="position: relative;">
      <img
        src="/assets/images/feedback-doctor-logo.svg"
        style="width: 100%;display: block;" />
      <button
        type="button"
        style="border: none;
          background-color: transparent;
          position: absolute;
          top: 10px;
          right: 25px;"
        aria-label="Close dialog"
        (click)="onSuccessDialogClose()">
        <img
          src="/assets/images/feedback-close-logo.png"
          style="width: 2.5rem;" />
      </button>
    </div>
    <h2 style="margin-top: 55px; text-align: center; font-weight: 700;">
      Thank you!
    </h2>
    <p style="margin-top: 20px; padding-left:20px; padding-bottom: 50px;">
      Your feedback is greatly appreciated, the help desk will contact you
      shortly.
    </p>`,
  styles: `p { text-align: center; }`,
  standalone: true,
})
export class FeedbackSendComponent {
  public faXmark = faXmark;
  public constructor(public dialog: MatDialog) {}

  public onSuccessDialogClose(): void {
    this.dialog.closeAll();
  }
}
