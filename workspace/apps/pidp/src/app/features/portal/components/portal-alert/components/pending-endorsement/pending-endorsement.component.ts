import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-pending-endorsement',
  standalone: true,
  imports: [CommonModule],
  template: `<p>
    ACTION REQUIRED:
    <span (click)="onClick()">Click here</span>
    to complete endorsement process
  </p>`,
  styles: [
    'span { cursor: pointer; text-decoration: underline; color: #1a5a96; }',
  ],
})
export class PendingEndorsementComponent {
  public constructor(private router: Router) {}
  public route = '/organization-info/endorsements';

  public onClick(): void {
    this.router.navigateByUrl(this.route);
  }
}
