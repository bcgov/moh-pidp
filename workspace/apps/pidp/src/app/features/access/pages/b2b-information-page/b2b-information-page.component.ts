import { CommonModule, NgOptimizedImage } from '@angular/common';
import { Component } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Router, RouterLink } from '@angular/router';

import { catchError, noop, of, tap } from 'rxjs';

import { NoContent } from '@bcgov/shared/data-access';
import { InjectViewportCssClassDirective } from '@bcgov/shared/ui';

import { PartyService } from '@app/core/party/party.service';
import { BreadcrumbComponent } from '@app/shared/components/breadcrumb/breadcrumb.component';

import { AccessRoutes } from '../../access.routes';
import { B2bInvitationResource } from './b2b-invitation-resource.service';

@Component({
  selector: 'app-b2b-information-page',
  standalone: true,
  imports: [
    BreadcrumbComponent,
    CommonModule,
    InjectViewportCssClassDirective,
    MatFormFieldModule,
    MatInputModule,
    NgOptimizedImage,
    RouterLink,
  ],
  templateUrl: './b2b-information-page.component.html',
  styleUrl: './b2b-information-page.component.scss',
})
export class B2bInformationPageComponent {
  public AccessRoutes = AccessRoutes;
  public breadcrumbsData: Array<{ title: string; path: string }> = [
    { title: 'Home', path: '' },
    {
      title: 'Access',
      path: AccessRoutes.routePath(AccessRoutes.ACCESS_REQUESTS),
    },
    { title: 'B2B Information', path: '' },
  ];

  public constructor(
    private router: Router,
    private b2bResource: B2bInvitationResource,
    private partyService: PartyService,
  ) {}

  public onNext(): void {
    this.router.navigate([AccessRoutes.routePath(AccessRoutes.IMMSBC)]);
  }

  public onContinue(): void {
    this.router.navigate([
      AccessRoutes.routePath(AccessRoutes.BC_PROVIDER_EDIT),
    ]);
  }

  public onInvite(userPrincipalName: string): void {
    this.b2bResource
      .inviteGuestAccount(this.partyService.partyId, userPrincipalName)
      .pipe(
        tap((_) => {
          console.info('User invited');
        }),
        catchError(() => {
          console.error('Failed to invite user');
          return of(noop());
        }),
      )
      .subscribe();
  }
}
