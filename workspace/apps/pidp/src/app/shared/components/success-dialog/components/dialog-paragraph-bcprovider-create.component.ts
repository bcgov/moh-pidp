import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-dialog-paragraph-bcprovider-create',
  template: ` <p>
    Password reset for <strong>{{ username }}</strong> has been created.<br />
    You will now have the option to sign in to our system with these
    credentials.
  </p>`,
  styles: ``,
})
export class DialogParagraphBcproviderCreateComponent {
  @Input() public username!: string;
}
