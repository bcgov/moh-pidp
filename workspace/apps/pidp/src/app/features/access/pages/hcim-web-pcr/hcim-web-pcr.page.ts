import { ClipboardModule } from '@angular/cdk/clipboard';
import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { AsyncPipe, NgOptimizedImage } from '@angular/common';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnDestroy, OnInit, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatStepperModule } from '@angular/material/stepper';
import { MatTooltipModule } from '@angular/material/tooltip';
import { RouterLink } from '@angular/router';

import {
  BehaviorSubject,
  Observable,
  Subject,
  catchError,
  noop,
  of,
  switchMap,
  takeUntil,
  tap,
} from 'rxjs';

import {
  LOADING_OVERLAY_DEFAULT_MESSAGE,
  LoadingOverlayService,
} from '@pidp/presentation';

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
import { PortalService } from '@app/features/portal/portal.service';
import { BreadcrumbComponent } from '@app/shared/components/breadcrumb/breadcrumb.component';

import { BcProviderEditResource } from '../../../accounts/pages/bc-provider-edit/bc-provider-edit-resource.service';
import { BcProviderEditInitialStateModel } from '../../../accounts/pages/bc-provider-edit/bc-provider-edit.page';
import { AccessRoutes } from '../../access.routes';
import { EnrolmentErrorComponent } from '../../components/enrolment-error/enrolment-error.component';
import { bcProviderTutorialLink } from '../provincial-attachment-system/provincial-attachment-system.constants';
import { HcimWebPcrResource } from './hcim-web-pcr-resource.service';
import { hcimWebPcrUrl, registriesConnectionsEmail } from './hcim-web-pcr-constants';

@Component({
  selector: 'app-hcim-web-pcr',
  standalone: true,
  templateUrl: './hcim-web-pcr.page.html',
  styleUrls: ['./hcim-web-pcr.page.scss'],
  imports: [
    AsyncPipe,
    BreadcrumbComponent,
    EnrolmentErrorComponent,
    MatButtonModule,
    MatIconModule,
    MatStepperModule,
    MatTooltipModule,
    ClipboardModule,
    InjectViewportCssClassDirective,
    RouterLink,
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
  private readonly portalService = inject(PortalService);
  private readonly partyService = inject(PartyService);
  private readonly resource = inject(HcimWebPcrResource);
  private readonly loadingOverlayService = inject(LoadingOverlayService);
  private readonly toastService = inject(ToastService);

  public readonly bcProvider$: BehaviorSubject<boolean> =
    new BehaviorSubject<boolean>(false);
  public destination$: Observable<Destination>;
  public readonly hcimWebPcr$: BehaviorSubject<boolean> =
    new BehaviorSubject<boolean>(true);
  public bcProviderStatusCode: number | undefined;
  public hcimWebPcrStatusCode: number | undefined;
  public accessRequestFailed = false;
  public enrolmentError = false;
  public bcProviderUsername = '';
  public logoutRedirectUrl: string;
  public bcProviderTutorial: string;
  public selectedIndex: number;
  private readonly lastSelectedIndex: number;
  private readonly getAccessStepIndex: number;
  public hasCpn: boolean | undefined;
  public Destination = Destination;
  public StatusCode = StatusCode;
  public AccessRoutes = AccessRoutes;
  public readonly hcimWebPcrUrl = hcimWebPcrUrl;
  public readonly registriesConnectionsEmail = registriesConnectionsEmail;
  private readonly destroy$ = new Subject<void>();
  public breadcrumbsData: Array<{ title: string; path: string }> = [
    { title: 'Home', path: '' },
    {
      title: 'Access',
      path: AccessRoutes.routePath(AccessRoutes.ACCESS_REQUESTS),
    },
    { title: 'Provincial Client Registry', path: '' },
  ];

  public constructor() {
    this.selectedIndex = -1;
    this.logoutRedirectUrl = `${this.config.applicationUrl}/`;
    this.bcProviderTutorial = bcProviderTutorialLink;
    this.getAccessStepIndex = 2;
    this.lastSelectedIndex = 3;
    this.destination$ = this.discoveryResource.getDestination(
      this.partyService.partyId,
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

  /**
   * Mirrors the server-side gate in the HcimWebPcr command handler: the section
   * is only AVAILABLE when the party holds a BC Services Card credential and is
   * in good standing with the PLR, and the grant additionally requires a
   * BCProvider credential. Keeping these aligned stops the button from being
   * clickable in states the API would reject.
   */
  public get getAccessDisabled(): boolean {
    return (
      this.bcProviderStatusCode !== StatusCode.COMPLETED ||
      this.hcimWebPcrStatusCode !== StatusCode.AVAILABLE
    );
  }

  public onRequestAccess(): void {
    this.loadingOverlayService.open(LOADING_OVERLAY_DEFAULT_MESSAGE);
    this.resource
      .requestAccess(this.partyService.partyId)
      .pipe(
        takeUntil(this.destroy$),
        tap(() => {
          this.loadingOverlayService.close();
          this.accessRequestFailed = false;
          this.enrolmentError = false;
          this.refreshStepperState();
        }),
        catchError((error: HttpErrorResponse) => {
          this.loadingOverlayService.close();
          if (error.status === HttpStatusCode.BadRequest) {
            this.enrolmentError = true;
            return of(noop());
          }
          this.accessRequestFailed = true;
          return of(noop());
        }),
      )
      .subscribe();
  }

  public ngOnInit(): void {
    this.refreshStepperState();
  }

  public ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private refreshStepperState(): void {
    this.handleStepperState(
      this.portalResource.getProfileStatus(this.partyService.partyId),
    );
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
          this.hcimWebPcr$.next(
            this.hcimWebPcrStatusCode !== StatusCode.COMPLETED,
          );
          if (
            this.hcimWebPcrStatusCode !== StatusCode.COMPLETED &&
            selectedIndex === this.lastSelectedIndex &&
            this.bcProviderStatusCode === StatusCode.COMPLETED
          ) {
            selectedIndex = this.getAccessStepIndex;
          }
          this.selectedIndex = selectedIndex;
          // Keep the dashboard card in step with the access just granted.
          this.portalService.updateState(profileStatus);
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

