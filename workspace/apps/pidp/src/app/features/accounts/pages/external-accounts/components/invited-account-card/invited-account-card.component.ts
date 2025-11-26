import { DatePipe } from '@angular/common';
import { Component, Input } from '@angular/core';

import { InjectViewportCssClassDirective } from '@bcgov/shared/ui';

@Component({
  selector: 'app-invited-account-card',
  imports: [InjectViewportCssClassDirective, DatePipe],
  templateUrl: './invited-account-card.component.html',
  styleUrl: './invited-account-card.component.scss',
  standalone: true,
})
export class InvitedAccountCardComponent {
  @Input() public invitedUserPrincipalName!: string;
  @Input() public invitedAt!: string;
}
