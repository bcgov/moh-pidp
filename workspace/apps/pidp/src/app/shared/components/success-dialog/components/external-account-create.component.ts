import { Component } from '@angular/core';

@Component({
  selector: 'app-dialog-external-account-create',
  template: ` <p>
    Thank you for providing OneHealthID with the information needed at this
    time.<br />
    You will now have the option to sign in to the following application with
    these credentials.
  </p>`,
  styles: `p { text-align: center;}`,
  standalone: true,
})
export class DialogExternalAccountCreateComponent {}
