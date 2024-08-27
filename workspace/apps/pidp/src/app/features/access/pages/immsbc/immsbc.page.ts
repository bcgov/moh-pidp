import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { AsyncPipe, NgIf, NgOptimizedImage } from '@angular/common';
import { AccessRoutes } from '../../access.routes';
import { AuthService } from '@app/features/auth/services/auth.service';
import { APP_CONFIG, AppConfig } from '@app/app.config';
import { ClipboardModule } from '@angular/cdk/clipboard';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatStepperModule } from '@angular/material/stepper';

import {
  BehaviorSubject,
  catchError,
  noop,
  of,
  Observable,
  switchMap,
  tap,
} from 'rxjs';


import {
  AlertComponent,
  InjectViewportCssClassDirective,
} from '@bcgov/shared/ui';

import {
  Destination,
  DiscoveryResource,
} from '@app/core/party/discovery-resource.service';
import {
  ImmsbcWebsite,
  bcProviderTutorialLink,
  immsBCUrl,
} from './immsbc.constants';

import { ProfileStatus } from '@app/features/portal/models/profile-status.model';
import { ToastService } from '@app/core/services/toast.service';
import { PartyService } from '@app/core/party/party.service';
import { BreadcrumbComponent } from '@app/shared/components/breadcrumb/breadcrumb.component';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';
import { PortalResource } from '@app/features/portal/portal-resource.service';

import { BcProviderEditResource } from '../bc-provider-edit/bc-provider-edit-resource.service';
import { EnrolmentErrorComponent } from '../../components/enrolment-error/enrolment-error.component';
import { BcProviderEditInitialStateModel } from '../bc-provider-edit/bc-provider-edit.page';
import { ImmsBCResource } from './immsbc-resource.service';

@Component({
  selector: 'app-immsbc',
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
  templateUrl: './immsbc.page.html',
  styleUrl: './immsbc.page.scss',
  providers: [
    {
      provide: STEPPER_GLOBAL_OPTIONS,
      useValue: { displayDefaultIndicatorType: false },
    },
  ],
})
export class ImmsbcPage implements OnInit {
  public bcProvider$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(
    false,
  );
  public destination$: Observable<Destination>;
  public immsbc$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(true);
  public bcProviderStatusCode: number | undefined;
  public immsbcStatusCode: number | undefined;
  public bcProviderUsername = '';
  public logoutRedirectUrl: string;
  public bcProviderTutorial: string;
  public selectedIndex: number;
  private readonly lastSelectedIndex: number;
  public hasCpn: boolean | undefined;
  public Destination = Destination;
  public StatusCode = StatusCode;
  public AccessRoutes = AccessRoutes;
  public title: string;
  public immsBCUrl: string;
  public completed: boolean | null;
  public accessRequestFailed: boolean;
  public enrolmentError: boolean;

  public breadcrumbsData: Array<{ title: string; path: string }> = [
    { title: 'Home', path: '' },
    {
      title: 'Access',
      path: AccessRoutes.routePath(AccessRoutes.ACCESS_REQUESTS),
    },
    { title: 'ImmsBC', path: '' },
  ];
  private readonly ImmsbcWebsite: string;

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private authService: AuthService,
    private bcProviderResource: BcProviderEditResource,
    private discoveryResource: DiscoveryResource,
    private portalResource: PortalResource,
    private partyService: PartyService,
    private router: Router,
    private resource: ImmsBCResource,
    private route: ActivatedRoute,
    private toastService: ToastService,
  ) {
    this.selectedIndex = -1;
    this.ImmsbcWebsite = ImmsbcWebsite;
    this.logoutRedirectUrl = `${this.config.applicationUrl}/`;
    this.bcProviderTutorial = bcProviderTutorialLink;
    this.lastSelectedIndex = 3;
    this.immsBCUrl = immsBCUrl;
    this.destination$ = this.discoveryResource.getDestination(
      this.partyService.partyId,
    );
    const routeData = this.route.snapshot.data;
    this.title = routeData.title;
    this.completed = false;
    this.accessRequestFailed = false;
    this.enrolmentError = false;
  }

  public navigateToSrcPath(): void {
    this.navigateToExternalUrl(this.ImmsbcWebsite);
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
          this.completed = true;
          this.enrolmentError = false;
        }),
        catchError((error: HttpErrorResponse) => {
          if (error.status === HttpStatusCode.BadRequest) {
            this.completed = false;
            this.enrolmentError = true;
            return of(noop());
          }
          this.accessRequestFailed = true;
          return of(noop());
        }),
      )
      .subscribe();
  }

  private handleStepperState(
    profileStatus$: Observable<ProfileStatus | null>,
  ): void {
    let selectedIndex = this.lastSelectedIndex;
    profileStatus$
      .pipe(
        tap((profileStatus: ProfileStatus | null) => {
          this.hasCpn = profileStatus?.status.collegeCertification.hasCpn;
          this.immsbcStatusCode = profileStatus?.status.immsbc.statusCode;
          this.bcProviderStatusCode =
            profileStatus?.status.bcProvider.statusCode;
          if (this.immsbcStatusCode === StatusCode.COMPLETED) {
            this.immsbc$.next(false);
          } else if (
            selectedIndex === this.lastSelectedIndex &&
            this.bcProviderStatusCode === StatusCode.COMPLETED
          ) {
            // IMMSBC step
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
    window.open(url, '_blank');
    this.router.navigateByUrl('/');
  }
}
