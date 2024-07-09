import { AsyncPipe } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { IsActiveMatchOptions } from '@angular/router';

import { Observable, map, tap } from 'rxjs';

import {
  DashboardHeaderConfig,
  DashboardMenuItem,
  DashboardRouteMenuItem,
  IDashboard,
} from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import { PartyService } from '@app/core/party/party.service';
import { AccessRoutes } from '@app/features/access/access.routes';
import { AuthService } from '@app/features/auth/services/auth.service';
import { AlertCode } from '@app/features/portal/enums/alert-code.enum';
import { PortalResource } from '@app/features/portal/portal-resource.service';
import { ProfileRoutes } from '@app/features/profile/profile.routes';

import { NavMenuComponent } from '../navbar-menu/nav-menu';

@Component({
  selector: 'app-portal-dashboard',
  templateUrl: './portal-dashboard.component.html',
  styleUrls: ['./portal-dashboard.component.scss'],
  standalone: true,
  imports: [AsyncPipe, NavMenuComponent],
})
export class PortalDashboardComponent implements IDashboard {
  public logoutRedirectUrl: string;
  public headerConfig: DashboardHeaderConfig;
  public brandConfig: { imgSrc: string; imgAlt: string };
  public showMenuItemIcons: boolean;
  public responsiveMenuItems: boolean;
  public menuItems: DashboardMenuItem[];
  public providerIdentitySupport: string;
  public collegeRoute: string = '';

  public alerts$: Observable<AlertCode[]>;

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private authService: AuthService,
    private partyService: PartyService,
    private resource: PortalResource,
  ) {
    this.logoutRedirectUrl = `${this.config.applicationUrl}/${this.config.routes.auth}`;
    this.headerConfig = { theme: 'light', allowMobileToggle: true };
    this.brandConfig = {
      imgSrc: '/assets/images/pidp-logo-white.svg',
      imgAlt: 'OneHealthID Service Logo',
    };
    this.showMenuItemIcons = true;
    this.responsiveMenuItems = false;
    this.menuItems = this.createMenuItems();
    this.providerIdentitySupport = this.config.emails.providerIdentitySupport;
    this.alerts$ = this.resource
      .getProfileStatus(this.partyService.partyId)
      .pipe(
        tap((profileStatus) => {
          this.collegeRoute = profileStatus?.status.collegeCertification
            .licenceDeclared
            ? ProfileRoutes.routePath(ProfileRoutes.COLLEGE_LICENCE_INFO)
            : ProfileRoutes.routePath(
                ProfileRoutes.COLLEGE_LICENCE_DECLARATION,
              );
        }),
        map((profileStatus) => profileStatus?.alerts ?? []),
      );
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
        'Access',
        {
          commands: [AccessRoutes.BASE_PATH, AccessRoutes.ACCESS_REQUESTS],
          linkActiveOptions,
        },
        'assignment',
      ),
      new DashboardRouteMenuItem(
        'Help',
        {
          commands: 'help',
          linkActiveOptions,
        },
        'help_outline',
      ),
    ];
  }
}
