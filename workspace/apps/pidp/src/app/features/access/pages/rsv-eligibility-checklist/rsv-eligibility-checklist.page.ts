import { ClipboardModule } from '@angular/cdk/clipboard';
import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { AsyncPipe, NgOptimizedImage } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatStepperModule } from '@angular/material/stepper';
import { MatTooltipModule } from '@angular/material/tooltip';
import { Router, RouterLink } from '@angular/router';

import { BehaviorSubject, Observable, of, switchMap, tap } from 'rxjs';

import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

import { InjectViewportCssClassDirective } from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import {
  Destination,
  DiscoveryResource,
} from '@app/core/party/discovery-resource.service';
import { PartyService } from '@app/core/party/party.service';
import { SnowplowService } from '@app/core/services/snowplow.service';
import { ToastService } from '@app/core/services/toast.service';
import { AuthService } from '@app/features/auth/services/auth.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';
import { ProfileStatus } from '@app/features/portal/models/profile-status.model';
import { PortalResource } from '@app/features/portal/portal-resource.service';
import { BreadcrumbComponent } from '@app/shared/components/breadcrumb/breadcrumb.component';

import { BcProviderEditResource } from '../../../accounts/pages/bc-provider-edit/bc-provider-edit-resource.service';
import { BcProviderEditInitialStateModel } from '../../../accounts/pages/bc-provider-edit/bc-provider-edit.page';
import { AccessRoutes } from '../../access.routes';
import {
  bcProviderTutorialLink,
  rsvEligibilityChecklistWebsite,
} from './rsv-eligibility-checklist.constants';

@Component({
  selector: 'app-rsv-eligibility-checklist',
  imports: [
    AsyncPipe,
    BreadcrumbComponent,
    MatButtonModule,
    MatIconModule,
    MatStepperModule,
    MatTooltipModule,
    ClipboardModule,
    InjectViewportCssClassDirective,
    RouterLink,
    FontAwesomeModule,
    NgOptimizedImage
],
  templateUrl: './rsv-eligibility-checklist.page.html',
  styleUrl: './rsv-eligibility-checklist.page.scss',
  providers: [
    {
      provide: STEPPER_GLOBAL_OPTIONS,
      useValue: { displayDefaultIndicatorType: false },
    },
  ],
})
export class RsvEligibilityChecklistPage implements OnInit {
  private readonly config = inject<AppConfig>(APP_CONFIG);
  private readonly authService = inject(AuthService);
  private readonly bcProviderResource = inject(BcProviderEditResource);
  private readonly discoveryResource = inject(DiscoveryResource);
  private readonly portalResource = inject(PortalResource);
  private readonly partyService = inject(PartyService);
  private readonly router = inject(Router);
  private readonly toastService = inject(ToastService);
  private readonly snowplowService = inject(SnowplowService);

  public bcProvider$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(
    false,
  );
  public destination$: Observable<Destination>;
  public rsv$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(true);
  public bcProviderStatusCode: number | undefined;
  public rsvStatusCode: number | undefined;
  public bcProviderUsername = '';
  public logoutRedirectUrl: string;
  public bcProviderTutorial: string;
  public selectedIndex: number;
  private readonly lastSelectedIndex: number;
  public hasCpn: boolean | undefined;
  public Destination = Destination;
  public StatusCode = StatusCode;
  public AccessRoutes = AccessRoutes;
  public breadcrumbsData: Array<{ title: string; path: string }> = [
    { title: 'Home', path: '' },
    {
      title: 'Access',
      path: AccessRoutes.routePath(AccessRoutes.ACCESS_REQUESTS),
    },
    { title: 'RSV Eligibility Checklist eForm', path: '' },
  ];
  private readonly rsvEligibilityChecklistWebsite: string;

  public constructor() {
    this.selectedIndex = -1;
    this.rsvEligibilityChecklistWebsite = rsvEligibilityChecklistWebsite;
    this.logoutRedirectUrl = `${this.config.applicationUrl}/`;
    this.bcProviderTutorial = bcProviderTutorialLink;
    this.lastSelectedIndex = 3;
    this.destination$ = this.discoveryResource.getDestination(
      this.partyService.partyId,
    );
  }

  public navigateToPath(): void {
    this.navigateToExternalUrl(this.rsvEligibilityChecklistWebsite);
    this.authService.logout(this.logoutRedirectUrl);
  }

  public onCopy(): void {
    this.toastService.openSuccessToast(
      'You have copied your BCProvider Username to clipboard.',
    );
  }

  public ngOnInit(): void {
    const profileStatus$ = this.portalResource.getProfileStatus(
      this.partyService.partyId,
    );

    this.handleStepperState(profileStatus$);
  }

  private handleStepperState(
    profileStatus$: Observable<ProfileStatus | null>,
  ): void {
    let selectedIndex = this.lastSelectedIndex;
    profileStatus$
      .pipe(
        tap((profileStatus: ProfileStatus | null) => {
          this.hasCpn = profileStatus?.status.collegeCertification.hasCpn;
          this.rsvStatusCode =
            profileStatus?.status.provincialAttachmentSystem.statusCode;
          this.bcProviderStatusCode =
            profileStatus?.status.bcProvider.statusCode;
          if (this.rsvStatusCode === StatusCode.COMPLETED) {
            this.rsv$.next(false);
          } else if (
            selectedIndex === this.lastSelectedIndex &&
            this.bcProviderStatusCode === StatusCode.COMPLETED
          ) {
            // RSV step
            selectedIndex = 1;
          }
          this.selectedIndex = selectedIndex;
        }),
        switchMap((): Observable<BcProviderEditInitialStateModel | null> => {
          if (this.bcProviderStatusCode === StatusCode.COMPLETED) {
            this.bcProvider$.next(true);
            return this.bcProviderResource.get(this.partyService.partyId);
          } else {
            if (selectedIndex === this.lastSelectedIndex) {
              // BCProvider step
              selectedIndex = 0;
            }
            this.selectedIndex = selectedIndex;
            return of(null);
          }
        }),
        tap((bcProviderObject: BcProviderEditInitialStateModel | null) => {
          if (bcProviderObject) {
            this.bcProviderUsername = bcProviderObject.bcProviderId;
          }
        }),
      )
      .subscribe();
  }

  private navigateToExternalUrl(url: string): void {
    this.snowplowService.trackLinkClick(this.rsvEligibilityChecklistWebsite);
    window.open(url, '_blank');
    this.router.navigateByUrl('/');
  }
}
