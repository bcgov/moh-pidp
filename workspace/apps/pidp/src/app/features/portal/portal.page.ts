import { ClipboardModule } from '@angular/cdk/clipboard';
import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { AsyncPipe, NgFor, NgIf } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatStepperModule } from '@angular/material/stepper';
import { MatTooltipModule } from '@angular/material/tooltip';
import { Router, RouterLink } from '@angular/router';

import { BehaviorSubject, Observable, of, switchMap, tap } from 'rxjs';

import {
  AlertComponent,
  AlertContentDirective,
  AnchorDirective,
  InjectViewportCssClassDirective,
  ScrollTargetComponent,
} from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import {
  Destination,
  DiscoveryResource,
} from '@app/core/party/discovery-resource.service';
import { PartyService } from '@app/core/party/party.service';
import { ToastService } from '@app/core/services/toast.service';
import { GetSupportComponent } from '@app/shared/components/get-support/get-support.component';
import { Role } from '@app/shared/enums/roles.enum';

import { AccessRoutes } from '../access/access.routes';
import { BcProviderEditResource } from '../access/pages/bc-provider-edit/bc-provider-edit-resource.service';
import { BcProviderEditInitialStateModel } from '../access/pages/bc-provider-edit/bc-provider-edit.page';
import { IdentityProvider } from '../auth/enums/identity-provider.enum';
import { AuthService } from '../auth/services/auth.service';
import { AuthorizedUserService } from '../auth/services/authorized-user.service';
import { BannerExpansionPanelComponent } from './components/banner-expansion-panel/banner-expansion-panel.component';
import { PortalAlertComponent } from './components/portal-alert/portal-alert.component';
import { PortalCarouselComponent } from './components/portal-carousel/portal-carousel.component';
import { StatusCode } from './enums/status-code.enum';
import { ProfileStatusAlert } from './models/profile-status-alert.model';
import { ProfileStatus } from './models/profile-status.model';
import { PortalResource } from './portal-resource.service';
import { bcProviderTutorialLink } from './portal.constants';
import { PortalService } from './portal.service';
import { IPortalSection } from './state/portal-section.model';
import { PortalState } from './state/portal-state.builder';

@Component({
  selector: 'app-portal',
  templateUrl: './portal.page.html',
  styleUrls: ['./portal.page.scss'],
  providers: [
    {
      provide: STEPPER_GLOBAL_OPTIONS,
      useValue: { displayDefaultIndicatorType: false },
    },
  ],
  standalone: true,
  imports: [
    AlertComponent,
    AlertContentDirective,
    AnchorDirective,
    AsyncPipe,
    BannerExpansionPanelComponent,
    ClipboardModule,
    GetSupportComponent,
    InjectViewportCssClassDirective,
    MatButtonModule,
    MatIconModule,
    MatStepperModule,
    MatTooltipModule,
    NgFor,
    NgIf,
    PortalAlertComponent,
    PortalCarouselComponent,
    RouterLink,
    ScrollTargetComponent,
  ],
})
export class PortalPage implements OnInit {
  /**
   * @description
   * State for driving the displayed groups and sections of
   * the portal.
   */
  public state$: Observable<PortalState>;
  /**
   * @description
   * List of HTTP response controlled alert messages for display
   * in the portal.
   */
  public alerts: ProfileStatusAlert[];

  public Role = Role;
  public bcProvider$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(
    false,
  );
  public rostering$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(
    true,
  );
  public pasPanelExpanded$: BehaviorSubject<boolean>;
  public bcProviderStatusCode: number | undefined;
  public rosteringStatusCode: number | undefined;
  public bcProviderUsername = '';
  public logoutRedirectUrl: string;
  public bcProviderTutorial: string;
  public selectedIndex: number;
  private readonly lastSelectedIndex: number;
  public hasCpn: boolean | undefined;
  public destination$: Observable<Destination>;
  public IdentityProvider = IdentityProvider;
  public identityProvider$: Observable<IdentityProvider>;
  public pasAllowedProviders: IdentityProvider[];
  public Destination = Destination;

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private bcProviderResource: BcProviderEditResource,
    private router: Router,
    private partyService: PartyService,
    private portalResource: PortalResource,
    private portalService: PortalService,
    private authService: AuthService,
    private authorizedUserService: AuthorizedUserService,
    private toastService: ToastService,
    private discoveryResource: DiscoveryResource,
  ) {
    this.state$ = this.portalService.state$;
    this.pasPanelExpanded$ = this.portalService.pasPanelExpanded$;
    this.alerts = [];
    this.logoutRedirectUrl = `${this.config.applicationUrl}/`;
    this.bcProviderTutorial = bcProviderTutorialLink;
    this.lastSelectedIndex = 3;
    this.selectedIndex = -1;
    this.identityProvider$ = this.authorizedUserService.identityProvider$;
    this.pasAllowedProviders = [
      IdentityProvider.BCSC,
      IdentityProvider.BC_PROVIDER,
    ];
    this.destination$ = this.discoveryResource.getDestination(
      this.partyService.partyId,
    );
  }

  public navigateToAccess(): void {
    this.router.navigateByUrl(
      AccessRoutes.routePath(AccessRoutes.ACCESS_REQUESTS),
    );
  }

  public navigateTo(): void {
    if (this.bcProviderStatusCode !== StatusCode.COMPLETED) {
      this.router.navigateByUrl('/access/bc-provider-application');
    } else if (
      this.bcProviderStatusCode === StatusCode.COMPLETED &&
      this.rosteringStatusCode === StatusCode.AVAILABLE
    ) {
      this.navigateToExternalUrl('https://bchealthprovider.ca');
      this.authService.logout(this.logoutRedirectUrl);
    }
  }

  public onScrollToAnchor(): void {
    this.router.navigate([], {
      fragment: 'access',
      queryParamsHandling: 'preserve',
    });
  }

  public onCardAction(section: IPortalSection): void {
    section.performAction();
  }

  public onCopy(): void {
    this.toastService.openSuccessToast(
      'You have copied your BCProvider Username to clipboard.',
    );
  }

  public onExpansionPanelToggle(expanded: boolean): void {
    this.portalService.updateIsPASExpanded(expanded);
  }

  public ngOnInit(): void {
    const profileStatus$ = this.portalResource.getProfileStatus(
      this.partyService.partyId,
    );

    this.updateStateAndAlerts(profileStatus$);
    this.handlePasBannerStatus(profileStatus$);
    this.handlePasBannerRosteringStatus(profileStatus$);
  }

  private updateStateAndAlerts(
    profileStatus$: Observable<ProfileStatus | null>,
  ): void {
    profileStatus$
      .pipe(
        tap((profileStatus: ProfileStatus | null) => {
          this.portalService.updateState(profileStatus);
          this.alerts = this.portalService.alerts;
        }),
      )
      .subscribe();
  }

  private handlePasBannerStatus(
    profileStatus$: Observable<ProfileStatus | null>,
  ): void {
    profileStatus$
      .pipe(
        switchMap(
          (
            profileStatus,
          ): Observable<BcProviderEditInitialStateModel | null> => {
            let selectedIndex = this.lastSelectedIndex;
            this.hasCpn = profileStatus?.status.collegeCertification.hasCpn;

            this.bcProviderStatusCode =
              profileStatus?.status.bcProvider.statusCode;
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
          },
        ),
        tap((bcProviderObject: BcProviderEditInitialStateModel | null) => {
          if (bcProviderObject) {
            this.bcProviderUsername = bcProviderObject.bcProviderId;
          }
        }),
      )
      .subscribe();
  }

  private handlePasBannerRosteringStatus(
    profileStatus$: Observable<ProfileStatus | null>,
  ): void {
    profileStatus$
      .pipe(
        tap((profileStatus: ProfileStatus | null) => {
          let selectedIndex = this.lastSelectedIndex;
          this.rosteringStatusCode =
            profileStatus?.status.primaryCareRostering.statusCode;
          if (this.rosteringStatusCode === StatusCode.AVAILABLE) {
            this.rostering$.next(false);
          } else if (selectedIndex === this.lastSelectedIndex) {
            // PAS step
            selectedIndex = 1;
          }
          this.selectedIndex = selectedIndex;
        }),
      )
      .subscribe();
  }

  private navigateToExternalUrl(url: string): void {
    window.open(url, '_blank');
    this.router.navigateByUrl('/');
  }
}
