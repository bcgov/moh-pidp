import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { BehaviorSubject, Observable, map, of, switchMap, tap } from 'rxjs';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import { PartyService } from '@app/core/party/party.service';
import { Role } from '@app/shared/enums/roles.enum';

import { BcProviderEditInitialStateModel } from '../access/pages/bc-provider-edit/bc-provider-edit.component';
import { BcProviderEditResource } from '../access/pages/bc-provider-edit/bc-provider-edit.resource';
import { AuthService } from '../auth/services/auth.service';
import { EndorsementsResource } from '../organization-info/pages/endorsements/endorsements-resource.service';
import { ProfileStatusAlert } from './models/profile-status-alert.model';
import { ProfileStatus } from './models/profile-status.model';
import { PortalResource } from './portal-resource.service';
import {
  bcProviderTutorialLink,
  collegeLicenceTutorialLink,
  personalInfoTutorialLink,
  uaaTutorialLink,
} from './portal.constants';
import { PortalService } from './portal.service';
import { IPortalSection } from './state/portal-section.model';
import { PortalState } from './state/portal-state.builder';

@Component({
  selector: 'app-portal',
  templateUrl: './portal.page.html',
  styleUrls: ['./portal.page.scss'],
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
  public demographics$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(
    false
  );
  public collegeLicence$: BehaviorSubject<boolean> =
    new BehaviorSubject<boolean>(false);
  public uaa$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  public bcProvider$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(
    false
  );
  public rostering$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(
    true
  );
  public demographicsStatusCode!: number | undefined;
  public collegeLicenceStatusCode!: number | undefined;
  public uaaStatusCode!: number | undefined;
  public bcProviderStatusCode!: number | undefined;
  public rosteringStatusCode!: number | undefined;
  public bcProviderUsername = '';
  public logoutRedirectUrl: string;
  public personalInfoTutorial: string;
  public collegeLicenceTutorial: string;
  public uaaTutorial: string;
  public bcProviderTutorial: string;

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private bcProviderResource: BcProviderEditResource,
    private router: Router,
    private partyService: PartyService,
    private portalResource: PortalResource,
    private portalService: PortalService,
    private endorsementsResource: EndorsementsResource,
    private activatedRoute: ActivatedRoute,
    private authService: AuthService
  ) {
    this.state$ = this.portalService.state$;
    this.alerts = [];
    this.logoutRedirectUrl = `${this.config.applicationUrl}/`;
    this.personalInfoTutorial = personalInfoTutorialLink;
    this.collegeLicenceTutorial = collegeLicenceTutorialLink;
    this.uaaTutorial = uaaTutorialLink;
    this.bcProviderTutorial = bcProviderTutorialLink;
  }

  public navigateTo(): void {
    this.getProfileStatus(this.partyService.partyId).subscribe(
      (profileStatus: ProfileStatus | null) => {
        this.demographicsStatusCode =
          profileStatus?.status.demographics.statusCode;
        this.collegeLicenceStatusCode =
          profileStatus?.status.collegeCertification.statusCode;
        this.uaaStatusCode =
          profileStatus?.status.userAccessAgreement.statusCode;
        this.bcProviderStatusCode = profileStatus?.status.bcProvider.statusCode;
        this.rosteringStatusCode =
          profileStatus?.status.primaryCareRostering.statusCode;

        if (this.demographicsStatusCode !== 2) {
          this.router.navigateByUrl('/profile/personal-information');
        } else if (
          this.demographicsStatusCode === 2 &&
          this.collegeLicenceStatusCode !== 2
        ) {
          this.router.navigateByUrl('/profile/college-licence-declaration');
        } else if (
          this.demographicsStatusCode === 2 &&
          this.collegeLicenceStatusCode === 2 &&
          this.uaaStatusCode !== 2
        ) {
          this.router.navigateByUrl('/profile/user-access-agreement');
        } else if (
          this.demographicsStatusCode === 2 &&
          this.collegeLicenceStatusCode === 2 &&
          this.uaaStatusCode === 2 &&
          this.bcProviderStatusCode !== 2
        ) {
          this.router.navigateByUrl('/access/bc-provider-application');
        } else if (
          this.demographicsStatusCode === 2 &&
          this.collegeLicenceStatusCode === 2 &&
          this.bcProviderStatusCode === 2 &&
          this.rosteringStatusCode === 1
        ) {
          this.navigateToExternalUrl('https://bchealthprovider.ca');
          this.authService.logout(this.logoutRedirectUrl);
        }
      }
    );
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

  public getProfileStatus(partyId: number): Observable<ProfileStatus | null> {
    return this.portalResource.getProfileStatus(partyId);
  }

  public ngOnInit(): void {
    this.handleLandingActions$()
      .pipe(
        switchMap(() =>
          this.portalResource.getProfileStatus(this.partyService.partyId)
        ),
        tap((profileStatus: ProfileStatus | null) => {
          this.portalService.updateState(profileStatus);
          this.alerts = this.portalService.alerts;
        })
      )
      .subscribe((profileStatus) => {
        this.demographicsStatusCode =
          profileStatus?.status.demographics.statusCode;
        if (this.demographicsStatusCode === 2) {
          this.demographics$.next(true);
        }
        this.collegeLicenceStatusCode =
          profileStatus?.status.collegeCertification.statusCode;
        if (this.collegeLicenceStatusCode === 2) {
          this.collegeLicence$.next(true);
        }
        this.uaaStatusCode =
          profileStatus?.status.userAccessAgreement.statusCode;
        if (this.uaaStatusCode === 2) {
          this.uaa$.next(true);
        }
        this.bcProviderStatusCode = profileStatus?.status.bcProvider.statusCode;
        if (this.bcProviderStatusCode === 2) {
          this.bcProvider$.next(true);
          this.bcProviderResource
            .get(this.partyService.partyId)
            .subscribe((bcProviderObject: BcProviderEditInitialStateModel) => {
              this.bcProviderUsername = bcProviderObject.bcProviderId;
            });
        }
        this.rosteringStatusCode =
          profileStatus?.status.primaryCareRostering.statusCode;
        if (this.rosteringStatusCode === 1) {
          this.rostering$.next(false);
        }
      });
  }

  public handleLandingActions$(): Observable<void> {
    const endorsementToken =
      this.activatedRoute.snapshot.queryParamMap.get('endorsement-token');

    if (!endorsementToken) {
      return of(undefined);
    }

    return this.endorsementsResource
      .receiveEndorsementRequest(this.partyService.partyId, endorsementToken)
      .pipe(
        map(() => {
          this.router.navigate([], {
            queryParams: {
              'endorsement-token': null,
            },
            queryParamsHandling: 'merge',
          });
        })
      );
  }

  private navigateToExternalUrl(url: string): void {
    window.open(url, '_blank');
    this.router.navigateByUrl('/');
  }
}
