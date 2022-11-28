import { Component, Inject, OnInit } from '@angular/core';
import { IsActiveMatchOptions } from '@angular/router';

import { BehaviorSubject, Observable, forkJoin, map } from 'rxjs';

import {
  DashboardHeaderConfig,
  DashboardMenuItem,
  DashboardRouteMenuItem,
  DashboardStateModel,
  IDashboard,
} from '@bcgov/shared/ui';
import { ArrayUtils } from '@bcgov/shared/utils';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import { PartyService } from '@app/core/party/party.service';
import { AccessTokenService } from '@app/features/auth/services/access-token.service';
import { AuthService } from '@app/features/auth/services/auth.service';
import { ProfileStatus } from '@app/features/portal/models/profile-status.model';
import { PortalResource } from '@app/features/portal/portal-resource.service';
import { PortalRoutes } from '@app/features/portal/portal.routes';
import { CollegeCertification } from '@app/features/profile/pages/college-licence/college-licence-declaration/college-certification.model';
import { CollegeLicenceInformationResource } from '@app/features/profile/pages/college-licence/college-licence-information/college-licence-information-resource.service';
import { LookupService } from '@app/modules/lookup/lookup.service';
import { PermissionsService } from '@app/modules/permissions/permissions.service';
import { Role } from '@app/shared/enums/roles.enum';

import {
  RouteChangeDetectedEventArg,
  RouteIdentificationService,
  knownRouteNames,
} from '../../services/route-identification.service';

@Component({
  selector: 'app-portal-dashboard',
  templateUrl: './portal-dashboard.component.html',
  styleUrls: ['./portal-dashboard.component.scss'],
})
export class PortalDashboardComponent implements IDashboard, OnInit {
  public logoutRedirectUrl: string;
  public username: Observable<string>;
  public headerConfig: DashboardHeaderConfig;
  public brandConfig: { imgSrc: string; imgAlt: string };
  public showMenuItemIcons: boolean;
  public responsiveMenuItems: boolean;
  public menuItems: DashboardMenuItem[];
  public showUxV2: boolean;
  public dashboardState: DashboardStateModel = {
    titleText: '',
    titleDescriptionText: '',
    menuItems: [],
    userProfileFullNameText: '',
    userProfileCollegeNameText: '',
  };
  public dashboardState$ = new BehaviorSubject<DashboardStateModel>(
    this.dashboardState
  );

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private authService: AuthService,
    private permissionsService: PermissionsService,
    accessTokenService: AccessTokenService,
    private routeIdentificationService: RouteIdentificationService,
    private portalResourceService: PortalResource,
    private partyService: PartyService,
    private collegeLicenceInformationService: CollegeLicenceInformationResource,
    private lookupService: LookupService
  ) {
    this.logoutRedirectUrl = `${this.config.applicationUrl}/${this.config.routes.auth}`;
    this.username = accessTokenService.decodeToken().pipe(
      map((token) => {
        return token?.name ?? '';
      })
    );
    this.headerConfig = { theme: 'light', allowMobileToggle: true };
    this.brandConfig = {
      imgSrc: '/assets/images/pidp-logo-white.svg',
      imgAlt: 'Provider Identity Portal Logo',
    };
    this.showMenuItemIcons = true;
    this.responsiveMenuItems = false;
    this.menuItems = this.createMenuItems();
    this.showUxV2 = false;

    this.routeIdentificationService.routeNameBroadcast$.subscribe((arg) =>
      this.onRouteNameChange(arg)
    );

    this.setDashboardState({
      ...this.dashboardState,
      menuItems: [...this.menuItems],
    });
  }
  public ngOnInit(): void {
    // Get profile status and college info.
    // TODO: Insert a caching layer so a full get is not always required.
    const partyId = this.partyService.partyId;

    // Use forkJoin to wait for both to return.
    forkJoin({
      profileStatus: this.portalResourceService.getProfileStatus(partyId),
      collegeInfo: this.collegeLicenceInformationService.get(partyId),
    }).subscribe((result) => {
      const fullNameText = this.getUserFullNameText(result.profileStatus);
      const collegeName = this.getCollegeName(result.collegeInfo);
      this.setDashboardState({
        ...this.dashboardState,
        userProfileFullNameText: fullNameText,
        userProfileCollegeNameText: collegeName,
      });
    });
  }
  private setDashboardState(newState: DashboardStateModel): void {
    this.dashboardState = { ...newState };
    this.dashboardState$.next(this.dashboardState);
  }

  private onRouteNameChange(arg: RouteChangeDetectedEventArg): void {
    // Map the route to the UX dashboard version to use
    switch (arg.routeName) {
      case knownRouteNames.profile.collegeLicenseInfo:
      case knownRouteNames.portal:
        this.showUxV2 = this.config.featureFlags?.isLayoutV2Enabled ?? false;
        break;
      default:
        this.showUxV2 = false;
        break;
    }
  }

  public onLogout(): void {
    this.authService.logout(this.logoutRedirectUrl);
  }

  private createMenuItems(): DashboardMenuItem[] {
    const linkActiveOptions = {
      matrixParams: 'exact',
      queryParams: 'exact',
      paths: 'exact',
      fragment: 'exact',
    } as IsActiveMatchOptions;
    return [
      new DashboardRouteMenuItem(
        'Profile',
        {
          commands: PortalRoutes.MODULE_PATH,
          extras: { fragment: 'profile' },
          linkActiveOptions,
        },
        'assignment_ind'
      ),
      ...ArrayUtils.insertResultIf<DashboardRouteMenuItem>(
        this.permissionsService.hasRole([Role.FEATURE_PIDP_DEMO]),
        () => [
          new DashboardRouteMenuItem(
            'Organization Info',
            {
              commands: PortalRoutes.MODULE_PATH,
              extras: { fragment: 'organization' },
              linkActiveOptions,
            },
            'corporate_fare'
          ),
        ]
      ),
      new DashboardRouteMenuItem(
        'Access to Systems',
        {
          commands: PortalRoutes.MODULE_PATH,
          extras: { fragment: 'access' },
          linkActiveOptions,
        },
        'assignment'
      ),
      ...ArrayUtils.insertResultIf<DashboardRouteMenuItem>(
        this.permissionsService.hasRole([Role.FEATURE_PIDP_DEMO]),
        () => [
          new DashboardRouteMenuItem(
            'Training',
            {
              commands: PortalRoutes.MODULE_PATH,
              extras: { fragment: 'training' },
              linkActiveOptions,
            },
            'school'
          ),
        ]
      ),
      new DashboardRouteMenuItem(
        'History',
        {
          commands: PortalRoutes.MODULE_PATH,
          extras: { fragment: 'history' },
          linkActiveOptions,
        },
        'restore'
      ),
      new DashboardRouteMenuItem(
        'Get Support',
        {
          commands: PortalRoutes.MODULE_PATH,
          extras: { fragment: 'support' },
          linkActiveOptions,
        },
        'help_outline'
      ),
    ];
  }
  private getUserFullNameText(profileStatus: ProfileStatus | null): string {
    if (profileStatus?.status.demographics.firstName) {
      const fullName = `${profileStatus?.status.demographics.firstName} ${profileStatus?.status.demographics.lastName}`;
      return fullName;
    } else {
      return '';
    }
  }
  private getCollegeName(collegeInfo: CollegeCertification[] | null): string {
    if (!collegeInfo || collegeInfo.length === 0) {
      return '';
    }
    const firstCollegeInfo = collegeInfo[0];
    if (!firstCollegeInfo.collegeId) {
      return '';
    }
    const codeNumber = parseInt(firstCollegeInfo.collegeId);
    const college = this.lookupService.colleges.find(
      (x) => x.code === codeNumber
    );
    return college?.name ?? '';
  }
}
