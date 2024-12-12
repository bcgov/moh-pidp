import { Component, Input } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-dialog-feedback-send',
  template: `
    <h1>
      <img src="/assets/images/feedback-doctor-logo.svg" style="width: 100%;">
    </h1>
    <h2 style="margin-top: 50px; margin-left: 80px;font-weight:700">

      Thank you!
    </h2>
    <p style="margin-top: 20px;">
    Your feedback is greatly appreciated,
    the help desk will contact you shortly.
  </p>`,
  styles: `p { text-align: center; }`,
  standalone: true,
})
export class FeedbackSendComponent {
  @Input() public username!: string;

  public constructor(
    public dialog: MatDialog,
  ) {}

  public onSuccessDialogClose(): void {
    this.dialog.closeAll();
  }
}
