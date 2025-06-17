import { ClipboardModule } from '@angular/cdk/clipboard';
import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { AsyncPipe, NgIf, NgOptimizedImage } from '@angular/common';
import { Component, Inject, OnInit, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule, MatIconRegistry } from '@angular/material/icon';
import { MatStepperModule } from '@angular/material/stepper';
import { MatTooltipModule } from '@angular/material/tooltip';
import { DomSanitizer } from '@angular/platform-browser';
import { Router, RouterLink } from '@angular/router';

import { BehaviorSubject, Observable, of, switchMap, tap } from 'rxjs';

import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faArrowRight } from '@fortawesome/free-solid-svg-icons';

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

import { AccessRoutes } from '../../access.routes';
import { BcProviderEditResource } from '../bc-provider-edit/bc-provider-edit-resource.service';
import { BcProviderEditInitialStateModel } from '../bc-provider-edit/bc-provider-edit.page';
import {
  bcProviderTutorialLink,
  provincialAttachmentSystemWebsite,
} from '../provincial-attachment-system/provincial-attachment-system.constants';

@Component({
  selector: 'app-ivf',
  standalone: true,
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
    NgIf,
    FontAwesomeModule,
    NgOptimizedImage,
  ],
  templateUrl: './ivf.page.html',
  styleUrl: './ivf.page.scss',
  providers: [
    {
      provide: STEPPER_GLOBAL_OPTIONS,
      useValue: { displayDefaultIndicatorType: false },
    },
  ],
})
export class IvfPage implements OnInit {
  public bcProvider$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(
    false,
  );
  public destination$: Observable<Destination>;
  public ivf$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(true);
  public bcProviderStatusCode: number | undefined;
  public ivfStatusCode: number | undefined;
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
    { title: 'IVF', path: '' },
  ];
  private readonly provincialAttachmentSystemWebsite: string;
  public sanitizer = inject(DomSanitizer);
  public matIconRegistry = inject(MatIconRegistry);
  public faArrowRight = faArrowRight;

  public constructor(
    @Inject(APP_CONFIG) private readonly config: AppConfig,
    private readonly authService: AuthService,
    private readonly bcProviderResource: BcProviderEditResource,
    private readonly discoveryResource: DiscoveryResource,
    private readonly portalResource: PortalResource,
    private readonly partyService: PartyService,
    private readonly router: Router,
    private readonly toastService: ToastService,
    private readonly snowplowService: SnowplowService,
  ) {
    this.registerSvgIcons();
    this.selectedIndex = -1;
    this.provincialAttachmentSystemWebsite = provincialAttachmentSystemWebsite;
    this.logoutRedirectUrl = `${this.config.applicationUrl}/`;
    this.bcProviderTutorial = bcProviderTutorialLink;
    this.lastSelectedIndex = 3;
    this.destination$ = this.discoveryResource.getDestination(
      this.partyService.partyId,
    );
  }

  public registerSvgIcons(): void {
    // Register each SVG icon with MatIconRegistry
    this.matIconRegistry.addSvgIcon(
      'ivf-icon',
      this.sanitizer.bypassSecurityTrustResourceUrl(
        './assets/images/icons/ivf-icon.svg',
      ),
    );
  }

  public navigateToPath(): void {
    this.navigateToExternalUrl(this.provincialAttachmentSystemWebsite);
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
          this.ivfStatusCode = profileStatus?.status.ivf.statusCode;
          this.bcProviderStatusCode =
            profileStatus?.status.bcProvider.statusCode;
          if (this.ivfStatusCode === StatusCode.COMPLETED) {
            this.ivf$.next(false);
          } else if (
            selectedIndex === this.lastSelectedIndex &&
            this.bcProviderStatusCode === StatusCode.COMPLETED
          ) {
            // IVF step
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
    this.snowplowService.trackLinkClick(this.provincialAttachmentSystemWebsite);
    window.open(url, '_blank');
    this.router.navigateByUrl('/');
  }
}
