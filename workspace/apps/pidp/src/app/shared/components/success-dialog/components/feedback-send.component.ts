import { Component, Input } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faXmark } from '@fortawesome/free-solid-svg-icons';
@Component({
  selector: 'app-dialog-feedback-send',
  template: `
    <div style="position: relative;">
      <img src="/assets/images/feedback-doctor-logo.svg" style="width: 100%;display: block;">
      <button
        type="button"
        aria-label="Close dialog"
        (click)="onSuccessDialogClose()"
        style="width: 2rem;
          height: 2rem;
          border-radius: 1rem;
          display: flex;
          justify-content: center;
          align-items: center;
          border: solid 1px pidp.$grey-40;
          background-color: transparent;
          position: absolute;
          top: 10px;
          right: 25px;">
        <fa-icon [icon]="faXmark" style="font-size: 1.5rem;
          color: pidp.$grey-40;">
        </fa-icon>
      </button>
    </div>
    <h2 style="margin-top: 35px; margin-left: 100px;font-weight:700">

      Thank you!
    </h2>
    <p style="margin-top: 20px; padding-left:20px">
    Your feedback is greatly appreciated,
    the help desk will contact you shortly.
  </p>`,
  styles: `p { text-align: center; }`,
  standalone: true,
  imports: [FaIconComponent]
})
export class FeedbackSendComponent {
  @Input() public username!: string;

  public faXmark = faXmark;
  public constructor(
    public dialog: MatDialog,
  ) {}

  public onSuccessDialogClose(): void {
    this.dialog.closeAll();
  }
}
