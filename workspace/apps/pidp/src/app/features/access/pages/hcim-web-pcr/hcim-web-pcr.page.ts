import { ClipboardModule } from '@angular/cdk/clipboard';
import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { AsyncPipe, NgOptimizedImage } from '@angular/common';
import { Component, OnDestroy, OnInit, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule, MatIconRegistry } from '@angular/material/icon';
import { MatStepperModule } from '@angular/material/stepper';
import { MatTooltipModule } from '@angular/material/tooltip';
import { DomSanitizer } from '@angular/platform-browser';
import { Router, RouterLink } from '@angular/router';

import {
  BehaviorSubject,
  Observable,
  Subject,
  of,
  switchMap,
  takeUntil,
  tap,
} from 'rxjs';

import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faArrowRight } from '@fortawesome/free-solid-svg-icons';

import { InjectViewportCssClassDirective } from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import {
  Destination,
  DiscoveryResource,
} from '@app/core/party/discovery-resource.service';
import { PartyService } from '@app/core/party/party.service';
import { ToastService } from '@app/core/services/toast.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';
import { ProfileStatus } from '@app/features/portal/models/profile-status.model';
import { PortalResource } from '@app/features/portal/portal-resource.service';
import { BreadcrumbComponent } from '@app/shared/components/breadcrumb/breadcrumb.component';

import { BcProviderEditResource } from '../../../accounts/pages/bc-provider-edit/bc-provider-edit-resource.service';
import { BcProviderEditInitialStateModel } from '../../../accounts/pages/bc-provider-edit/bc-provider-edit.page';
import { AccessRoutes } from '../../access.routes';
import { bcProviderTutorialLink } from '../provincial-attachment-system/provincial-attachment-system.constants';
import { hcimWebPcrUrl } from './hcim-web-pcr-constants';

@Component({
  selector: 'app-hcim-web-pcr',
  standalone: true,
  templateUrl: './hcim-web-pcr.page.html',
  styleUrls: ['./hcim-web-pcr.page.scss'],
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
    NgOptimizedImage,
  ],
  providers: [
    {
      provide: STEPPER_GLOBAL_OPTIONS,
      useValue: { displayDefaultIndicatorType: false },
    },
  ],
})
export class HcimWebPcrPage implements OnInit, OnDestroy {
  private readonly config = inject<AppConfig>(APP_CONFIG);
  private readonly bcProviderResource = inject(BcProviderEditResource);
  private readonly discoveryResource = inject(DiscoveryResource);
  private readonly portalResource = inject(PortalResource);
  private readonly partyService = inject(PartyService);
  private readonly router = inject(Router);
  private readonly toastService = inject(ToastService);

  public bcProvider$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  public destination$: Observable<Destination>;
  public hcimWebPcr$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(true);
  public bcProviderStatusCode: number | undefined;
  public hcimWebPcrStatusCode: number | undefined;
  public bcProviderUsername = '';
  public logoutRedirectUrl: string;
  public bcProviderTutorial: string;
  public selectedIndex: number;
  private readonly lastSelectedIndex: number;
  public hasCpn: boolean | undefined;
  public Destination = Destination;
  public StatusCode = StatusCode;
  public AccessRoutes = AccessRoutes;
  public readonly hcimWebPcrUrl = hcimWebPcrUrl;
  private destroy$ = new Subject<void>();
  public breadcrumbsData: Array<{ title: string; path: string }> = [
    { title: 'Home', path: '' },
    {
      title: 'Access',
      path: AccessRoutes.routePath(AccessRoutes.ACCESS_REQUESTS),
    },
    { title: 'HCIM Web (Provincial Client Registry-PCR)', path: '' },
  ];
  public sanitizer = inject(DomSanitizer);
  public matIconRegistry = inject(MatIconRegistry);
  public faArrowRight = faArrowRight;

  public constructor() {
    this.registerSvgIcons();
    this.selectedIndex = -1;
    this.logoutRedirectUrl = `${this.config.applicationUrl}/`;
    this.bcProviderTutorial = bcProviderTutorialLink;
    this.lastSelectedIndex = 2;
    this.destination$ = this.discoveryResource.getDestination(
      this.partyService.partyId,
    );
  }

  public registerSvgIcons(): void {
    this.matIconRegistry.addSvgIcon(
      'hcim-icon',
      this.sanitizer.bypassSecurityTrustResourceUrl(
        './assets/images/icons/perm-contact-calendar.svg',
      ),
    );
  }

  public navigateToPath(): void {
    window.open(hcimWebPcrUrl, '_blank');
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

  public ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private handleStepperState(
    profileStatus$: Observable<ProfileStatus | null>,
  ): void {
    let selectedIndex = this.lastSelectedIndex;
    profileStatus$
      .pipe(
        takeUntil(this.destroy$),
        tap((profileStatus: ProfileStatus | null) => {
          this.hasCpn = profileStatus?.status.collegeCertification.hasCpn;
          this.hcimWebPcrStatusCode = profileStatus?.status.hcimWebPcr?.statusCode;
          this.bcProviderStatusCode =
            profileStatus?.status.bcProvider.statusCode;
          if (this.hcimWebPcrStatusCode === StatusCode.COMPLETED) {
            this.hcimWebPcr$.next(false);
          } else if (
            selectedIndex === this.lastSelectedIndex &&
            this.bcProviderStatusCode === StatusCode.COMPLETED
          ) {
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
}

