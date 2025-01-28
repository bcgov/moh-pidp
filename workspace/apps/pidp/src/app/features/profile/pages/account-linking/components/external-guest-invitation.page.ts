import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

import { InjectViewportCssClassDirective } from '@bcgov/shared/ui';

@Component({
  selector: 'app-external-guest-invitation',
  templateUrl: './external-guest-invitation.page.html',
  styleUrl: './external-guest-invitation.page.scss',
  standalone: true,
  imports: [CommonModule, InjectViewportCssClassDirective],
})
export class ExternalGuestInvitationComponent {

  public constructor() {}
}
