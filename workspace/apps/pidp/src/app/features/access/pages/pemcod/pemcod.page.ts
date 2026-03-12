import { ClipboardModule } from '@angular/cdk/clipboard';
import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { AsyncPipe, NgIf, NgOptimizedImage } from '@angular/common';
import { Component, Inject, OnDestroy, OnInit, inject } from '@angular/core';
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
import { pemcodUATWebsite } from './pemcod.constants';

@Component({
  selector: 'app-pemcod',
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
  templateUrl: './pemcod.page.html',
  styleUrl: './pemcod.page.scss',
  providers: [
    {
      provide: STEPPER_GLOBAL_OPTIONS,
      useValue: { displayDefaultIndicatorType: false },
    },
  ],
})
export class PemcodPage implements OnInit, OnDestroy {
  public bcProvider$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(
    false,
  );
  public destination$: Observable<Destination>;
  public pemcod$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(true);
  public bcProviderStatusCode: number | undefined;
  public pemcodStatusCode: number | undefined;
  public bcProviderUsername = '';
  public logoutRedirectUrl: string;
  public bcProviderTutorial: string;
  public selectedIndex: number;
  private readonly lastSelectedIndex: number;
  public hasCpn: boolean | undefined;
  public Destination = Destination;
  public StatusCode = StatusCode;
  public AccessRoutes = AccessRoutes;
  private destroy$ = new Subject<void>();
  public breadcrumbsData: Array<{ title: string; path: string }> = [
    { title: 'Home', path: '' },
    {
      title: 'Access',
      path: AccessRoutes.routePath(AccessRoutes.ACCESS_REQUESTS),
    },
    { title: `Practitioner's eMCOD`, path: '' },
  ];
  public sanitizer = inject(DomSanitizer);
  public matIconRegistry = inject(MatIconRegistry);
  public faArrowRight = faArrowRight;

  public constructor(
    @Inject(APP_CONFIG) private readonly config: AppConfig,
    private readonly bcProviderResource: BcProviderEditResource,
    private readonly discoveryResource: DiscoveryResource,
    private readonly portalResource: PortalResource,
    private readonly partyService: PartyService,
    private readonly router: Router,
    private readonly toastService: ToastService,
  ) {
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
    // Register each SVG icon with MatIconRegistry
    this.matIconRegistry.addSvgIcon(
      'pemcod-icon',
      this.sanitizer.bypassSecurityTrustResourceUrl(
        './assets/images/icons/credit-card.svg',
      ),
    );
  }

  public navigateToPath(): void {
    window.open(pemcodUATWebsite, '_blank');
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
          this.pemcodStatusCode = profileStatus?.status.pemcod.statusCode;
          this.bcProviderStatusCode =
            profileStatus?.status.bcProvider.statusCode;
          if (this.pemcodStatusCode === StatusCode.COMPLETED) {
            this.pemcod$.next(false);
          } else if (
            selectedIndex === this.lastSelectedIndex &&
            this.bcProviderStatusCode === StatusCode.COMPLETED
          ) {
            // PEMCOD step
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
}
