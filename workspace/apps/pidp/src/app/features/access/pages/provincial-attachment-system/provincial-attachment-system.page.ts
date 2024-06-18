import { ClipboardModule } from '@angular/cdk/clipboard';
import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { AsyncPipe, NgIf, NgOptimizedImage } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatStepperModule } from '@angular/material/stepper';
import { MatTooltipModule } from '@angular/material/tooltip';
import { Router, RouterLink } from '@angular/router';

import { BehaviorSubject, Observable, of, switchMap, tap } from 'rxjs';

import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faAngleRight } from '@fortawesome/free-solid-svg-icons';
import { NavigationService } from '@pidp/presentation';

import { InjectViewportCssClassDirective } from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import {
  Destination,
  DiscoveryResource,
} from '@app/core/party/discovery-resource.service';
import { PartyService } from '@app/core/party/party.service';
import { ToastService } from '@app/core/services/toast.service';
import { AuthService } from '@app/features/auth/services/auth.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';
import { ProfileStatus } from '@app/features/portal/models/profile-status.model';
import { PortalResource } from '@app/features/portal/portal-resource.service';
import { PortalService } from '@app/features/portal/portal.service';

import { BcProviderEditResource } from '../bc-provider-edit/bc-provider-edit-resource.service';
import { BcProviderEditInitialStateModel } from '../bc-provider-edit/bc-provider-edit.page';
import {
  bcProviderTutorialLink,
  provincialAttachmentSystemWebsite,
} from './provincial-attachment-system.constants';

@Component({
  selector: 'app-provincial-attachment-system',
  standalone: true,
  imports: [
    AsyncPipe,
    MatButtonModule,
    MatIconModule,
    MatStepperModule,
    MatTooltipModule,
    ClipboardModule,
    InjectViewportCssClassDirective,
    RouterLink,
    NgIf,
    FontAwesomeModule,
    NgOptimizedImage,
  ],
  templateUrl: './provincial-attachment-system.page.html',
  styleUrl: './provincial-attachment-system.page.scss',
  providers: [
    {
      provide: STEPPER_GLOBAL_OPTIONS,
      useValue: { displayDefaultIndicatorType: false },
    },
  ],
})
export class ProvincialAttachmentSystemPage implements OnInit {
  public bcProvider$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(
    false,
  );
  public destination$: Observable<Destination>;
  public pas$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(true);
  public bcProviderStatusCode: number | undefined;
  public pasStatusCode: number | undefined;
  public bcProviderUsername = '';
  public logoutRedirectUrl: string;
  public bcProviderTutorial: string;
  public selectedIndex: number;
  private readonly lastSelectedIndex: number;
  public hasCpn: boolean | undefined;
  public Destination = Destination;
  public StatusCode = StatusCode;
  public faAngleRight = faAngleRight;

  private readonly provincialAttachmentSystemWebsite: string;

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private authService: AuthService,
    private bcProviderResource: BcProviderEditResource,
    private discoveryResource: DiscoveryResource,
    private navigationService: NavigationService,
    private portalResource: PortalResource,
    private portalService: PortalService,
    private partyService: PartyService,
    private router: Router,
    private toastService: ToastService,
  ) {
    this.selectedIndex = -1;
    this.provincialAttachmentSystemWebsite = provincialAttachmentSystemWebsite;
    this.logoutRedirectUrl = `${this.config.applicationUrl}/`;
    this.bcProviderTutorial = bcProviderTutorialLink;
    this.lastSelectedIndex = 3;
    this.destination$ = this.discoveryResource.getDestination(
      this.partyService.partyId,
    );
  }

  public navigateTo(): void {
    this.navigateToExternalUrl(this.provincialAttachmentSystemWebsite);
    this.authService.logout(this.logoutRedirectUrl);
  }

  public onCopy(): void {
    this.toastService.openSuccessToast(
      'You have copied your BCProvider Username to clipboard.',
    );
  }

  public onBack(): void {
    this.navigationService.navigateToRoot();
  }

  public ngOnInit(): void {
    const profileStatus$ = this.portalResource.getProfileStatus(
      this.partyService.partyId,
    );

    this.updateState(profileStatus$);
    this.handlePasBannerStatus(profileStatus$);
    this.handlePasStatus(profileStatus$);
  }

  private updateState(profileStatus$: Observable<ProfileStatus | null>): void {
    profileStatus$
      .pipe(
        tap((profileStatus: ProfileStatus | null) => {
          this.portalService.updateState(profileStatus);
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

  private handlePasStatus(
    profileStatus$: Observable<ProfileStatus | null>,
  ): void {
    profileStatus$
      .pipe(
        tap((profileStatus: ProfileStatus | null) => {
          let selectedIndex = this.lastSelectedIndex;
          this.pasStatusCode =
            profileStatus?.status.provincialAttachmentSystem.statusCode;
          if (this.pasStatusCode === StatusCode.AVAILABLE) {
            this.pas$.next(false);
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
