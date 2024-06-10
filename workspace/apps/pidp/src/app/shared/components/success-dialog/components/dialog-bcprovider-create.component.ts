import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-dialog-bcprovider-create',
  template: ` <p>
    BC Provider account <strong>{{ username }}</strong> has been created.<br />
    You will now have the option to sign in to our system with these
    credentials.<br />
    Please note, this is not an email address. <br />You will not be able to
    send or receive any emails from this credential.
  </p>`,
  styles: `p { text-align: center;}`,
  standalone: true,
})
export class DialogBcproviderCreateComponent {
  @Input() public username!: string;
}
