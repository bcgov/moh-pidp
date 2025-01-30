import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

import { InjectViewportCssClassDirective } from '@bcgov/shared/ui';
import { BreadcrumbComponent } from "../../../../../shared/components/breadcrumb/breadcrumb.component";
import { AccessRoutes } from '@app/features/access/access.routes';
import { ProfileRoutes } from '@app/features/profile/profile.routes';

@Component({
  selector: 'app-external-guest-invitation',
  templateUrl: './external-guest-invitation.page.html',
  styleUrl: './external-guest-invitation.page.scss',
  standalone: true,
  imports: [CommonModule, InjectViewportCssClassDirective, BreadcrumbComponent],
})
export class ExternalGuestInvitationComponent {

  public breadcrumbsData: Array<{ title: string; path: string }> = [
    { title: 'Home', path: '' },
    {
      title: 'Access',
      path: AccessRoutes.routePath(AccessRoutes.ACCESS_REQUESTS),
    },
    {
      title: 'Account Linking',
      path: ProfileRoutes.routePath(ProfileRoutes.ACCOUNT_LINKING)
    },
    { title: 'External Guest Invitation', path: ''},
  ];
  public constructor() {}
}
