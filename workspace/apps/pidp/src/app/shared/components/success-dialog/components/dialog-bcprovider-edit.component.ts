import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-dialog-bcprovider-edit',
  template: ` <p>
    Password reset for <strong>{{ username }}</strong> has been created.<br />
    You will now have the option to sign in to our system with these
    credentials.
  </p>`,
  styles: `p { text-align: center;}`,
  standalone: true,
})
export class DialogBcproviderEditComponent {
  @Input() public username!: string;
}
