import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-pending-endorsement',
  template: `<p>
    ACTION REQUIRED:
    <span (click)="onClick()">Click here</span>
    to complete endorsement process
  </p>`,
  styles: [
    'span { cursor: pointer; text-decoration: underline; color: #0d6efd; }',
    'span:hover { color: #0a58ca; }',
  ],
  standalone: true,
})
export class PendingEndorsementComponent {
  public constructor(private router: Router) {}
  private route = '/organization-info/endorsements';

  public onClick(): void {
    this.router.navigateByUrl(this.route);
  }
}
