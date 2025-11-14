import { Component } from '@angular/core';

import { InjectViewportCssClassDirective } from '@bcgov/shared/ui';

@Component({
  selector: 'app-invited-account-card',
  imports: [InjectViewportCssClassDirective],
  templateUrl: './invited-account-card.component.html',
  styleUrl: './invited-account-card.component.scss',
})
export class InvitedAccountCardComponent {}
