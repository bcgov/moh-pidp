import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-dialog-feedback-send',
  template: `
    <div>
      <img src="/assets/images/feedback-submit-success-logo.svg">
    </div>
    <h1>
      Thank you!
    </h1>
    <p>
    Your feedback is greatly appreciated,
    the help desk will contact you shortly.
  </p>`,
  styles: `p { text-align: center;}`,
  standalone: true,
})
export class FeedbackSendComponent {
  @Input() public username!: string;
}
