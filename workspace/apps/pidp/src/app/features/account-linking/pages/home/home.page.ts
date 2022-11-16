import { Component } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-account-linking-home-page',
  templateUrl: './home.page.html',
  styleUrls: ['./home.page.scss'],
})
export class AccountLinkingHomePage {
  public messages: string[] = [];
  public constructor(private snackBar: MatSnackBar) {}
  public onLinkClick(): void {
    this.writeMessage('Starting to do something');
    this.writeMessage('Finished doing something');
    this.snackBar.open('Just kidding!', 'OK');
  }
  private writeMessage(s: string): void {
    this.messages.push(s);
  }
}
