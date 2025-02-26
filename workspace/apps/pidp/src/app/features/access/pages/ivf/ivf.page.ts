import { ClipboardModule } from '@angular/cdk/clipboard';
import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { AsyncPipe, NgIf, NgOptimizedImage } from '@angular/common';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatStepperModule } from '@angular/material/stepper';
import { MatTooltipModule } from '@angular/material/tooltip';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';

import {
  BehaviorSubject,
  Observable,
  catchError,
  noop,
  of,
  switchMap,
  tap,
} from 'rxjs';

import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

import {
  AlertComponent,
  InjectViewportCssClassDirective,
} from '@bcgov/shared/ui';

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
import { BreadcrumbComponent } from '@app/shared/components/breadcrumb/breadcrumb.component';

import { AccessRoutes } from '../../access.routes';
import { EnrolmentErrorComponent } from '../../components/enrolment-error/enrolment-error.component';
import { BcProviderEditResource } from '../bc-provider-edit/bc-provider-edit-resource.service';
import { BcProviderEditInitialStateModel } from '../bc-provider-edit/bc-provider-edit.page';
import { IvfResource } from './ivf-resource.service';
import {
  IvfWebsite,
  bcProviderTutorialLink,
  ivfUrl,
} from './ivf.constants';

@Component({
  selector: 'app-ivf',
  standalone: true,
  imports: [
    AlertComponent,
    AsyncPipe,
    BreadcrumbComponent,
    ClipboardModule,
    EnrolmentErrorComponent,
    FontAwesomeModule,
    InjectViewportCssClassDirective,
    MatButtonModule,
    MatIconModule,
    MatStepperModule,
    MatTooltipModule,
    RouterLink,
    NgIf,
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
  public bcProviderStatusCodeNumber: number | undefined;
  public ivfStatusCode: number | undefined;
  public bcProviderUsername = '';
  public logoutRedirectUrl: string;
  public bcProviderTutorial: string;
  public selectedIndex: number;
  private readonly lastSelectedIndex: number;
  public hasCpn: boolean | undefined;
  public Destination = Destination;
  public statusCode = StatusCode;
  public AccessRoutes = AccessRoutes;
  public title: string;
  public ivfUrl: string;
  public completedStatus: boolean | null;
  public accessRequestFailedFlag: boolean;
  public enrolmentErrorFlag: boolean;

  public breadcrumbsData: Array<{ title: string; path: string }> = [
    { title: 'Home', path: '' },
    {
      title: 'Access',
      path: AccessRoutes.routePath(AccessRoutes.ACCESS_REQUESTS),
    },
    { title: 'Ivf', path: '' },
  ];
  private readonly IvfWebsite: string;

  public constructor(
    @Inject(APP_CONFIG) private readonly config: AppConfig,
    private readonly authService: AuthService,
    private readonly bcProviderResource: BcProviderEditResource,
    private readonly discoveryResource: DiscoveryResource,
    private readonly portalResource: PortalResource,
    private readonly partyService: PartyService,
    private readonly router: Router,
    private readonly resource: IvfResource,
    private readonly route: ActivatedRoute,
    private readonly toastService: ToastService,
  ) {
    this.selectedIndex = -1;
    this.IvfWebsite = IvfWebsite;
    this.logoutRedirectUrl = `${this.config.applicationUrl}/`;
    this.bcProviderTutorial = bcProviderTutorialLink;
    this.lastSelectedIndex = 3;
    this.ivfUrl = ivfUrl;
    this.destination$ = this.discoveryResource.getDestination(
      this.partyService.partyId,
    );
    const routeData = this.route.snapshot.data;
    this.title = routeData.title;
    this.completedStatus = routeData.ivfStatusCode === StatusCode.COMPLETED;
    this.accessRequestFailedFlag = false;
    this.enrolmentErrorFlag = false;
  }

  public navigateToSrcPath(): void {
    this.navigateToUrl(this.IvfWebsite);
    this.authService.logout(this.logoutRedirectUrl);
  }

  public onCopyUserName(): void {
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

  public onRequestAccess(): void {
    this.resource
      .requestAccess(this.partyService.partyId)
      .pipe(
        tap(() => {
          this.completedStatus = true;
          this.enrolmentErrorFlag = false;
        }),
        catchError((error: HttpErrorResponse) => {
          if (error.status === HttpStatusCode.BadRequest) {
            this.completedStatus = false;
            this.enrolmentErrorFlag = true;
            return of(noop());
          }
          this.accessRequestFailedFlag = true;
          return of(noop());
        }),
      )
      .subscribe();
  }

  private handleStepperState(
    profileStatus$: Observable<ProfileStatus | null>,
  ): void {
    let foundIndex = this.lastSelectedIndex;
    profileStatus$
      .pipe(
        tap((profileStatus: ProfileStatus | null) => {
          this.hasCpn = profileStatus?.status.collegeCertification.hasCpn;
          this.ivfStatusCode = profileStatus?.status.ivf.statusCode;
          this.bcProviderStatusCodeNumber =
            profileStatus?.status.bcProvider.statusCode;
          if (
            this.ivfStatusCode === StatusCode.AVAILABLE &&
            this.bcProviderStatusCodeNumber === StatusCode.COMPLETED
          ) {
            this.ivf$.next(false);
          } else if (
            foundIndex === this.lastSelectedIndex &&
            this.bcProviderStatusCodeNumber === StatusCode.COMPLETED
          ){
            foundIndex = 1;
            this.completedStatus = profileStatus?.status.ivf.isComplete
              ? true
              : false
          }
          this.selectedIndex = foundIndex;
        }),
        switchMap((): Observable<BcProviderEditInitialStateModel | null> => {
          if (this.bcProviderStatusCodeNumber === StatusCode.COMPLETED) {
            this.bcProvider$.next(true);
            return this.bcProviderResource.get(this.partyService.partyId);
          } else {
            if (foundIndex === this.lastSelectedIndex) {
              foundIndex = 0;
            }
            this.selectedIndex = foundIndex;
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

  private navigateToUrl(urlLink: string): void {
    window.open(urlLink, '_blank');
    this.router.navigateByUrl('/');
  }
}
