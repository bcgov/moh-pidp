import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';

import { startWith } from 'rxjs';

import { DeviceResolution } from '@core/enums/device-resolution.enum';
import { ViewportService } from '@core/services/viewport.service';

import {
  DashboardMenuItem,
  DashboardRouteMenuItem,
} from '../../models/dashboard-menu-item.model';
import { DashboardSidenavProps } from '../../models/dashboard-sidenav-props.model';
import { DashboardHeaderConfig } from '../dashboard-header/dashboard-header.component';

// TODO make this fully presentational and move out into libs
@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export class DashboardComponent implements OnInit {
  /**
   * @description
   * Dashboard header configuration for theming.
   */
  @Input() public headerConfig!: DashboardHeaderConfig;
  /**
   * @description
   * Dashboard sidenav configuration for theming.
   */
  @Input() public sideNavConfig!: { imgSrc: string; imgAlt: string };
  /**
   * @description
   * Show the sidenav default branding.
   */
  @Input() public showBrand: boolean;
  /**
   * @description
   * List of dashboard details used to populate the side navigation
   * links for routing within the application.
   */
  @Input() public menuItems!: DashboardMenuItem[];
  /**
   * @description
   * Whether the dashboard menu items are responsive, and collapse
   * on mobile viewports.
   */
  @Input() public responsiveMenuItems: boolean;
  /**
   * @description
   * Whether the dashboard menu items should display their icons.
   */
  @Input() public showMenuItemIcons: boolean;
  /**
   * @description
   * Redirect URL after logout.
   */
  @Input() public logoutRedirectUrl!: string;
  /**
   * @description
   * Side navigation reference.
   */
  @ViewChild('sidenav') public sideNav!: MatSidenav;

  public sideNavProps!: DashboardSidenavProps;
  public username!: string;

  public constructor(
    // TODO make more generic and pass in authorized user so dependency can be dropped
    // private authService: AuthService,
    private viewportService: ViewportService
  ) {
    // TODO make this generic and remove defaults for config to for wrapping of the component
    this.headerConfig = {
      theme: 'blue',
      showMobileToggle: true,
    };
    // TODO make this generic and remove defaults for config to for wrapping of the component
    this.sideNavConfig = {
      imgSrc: '',
      imgAlt: '',
    };

    this.showBrand = false;
    // TODO decouple these from controlling the default width of the sidenav based on list width
    this.responsiveMenuItems = false;
    this.showMenuItemIcons = false;
  }

  // TODO material might have a service now that makes this obsolete
  public get isMobile(): boolean {
    return this.viewportService.isMobile;
  }

  // TODO material might have a service now that makes this obsolete
  public get isDesktop(): boolean {
    return this.viewportService.isDesktop || this.viewportService.isWideDesktop;
  }

  public onAction(dashboardMenuItem: DashboardMenuItem): void {
    // Close on mobile to prevent blocking the screen when routing
    if (
      dashboardMenuItem instanceof DashboardRouteMenuItem &&
      this.viewportService.isMobile
    ) {
      this.sideNav.close();
    }
  }

  public onLogout(): void {
    // const routePath = this.logoutRedirectUrl ?? this.config.loginRedirectUrl;
    // this.authService.logout(routePath);
  }

  public ngOnInit(): void {
    if (!this.showMenuItemIcons) {
      // Cannot be responsive icons are not being shown
      this.responsiveMenuItems = false;
    }

    // TODO make more generic and pass in authorized user
    // Set the authenticated username for the application header
    // this.authService
    //   .getUser$()
    //   .subscribe(
    //     ({ firstName, lastName }: BcscUser) =>
    //       (this.username = `${firstName} ${lastName}`)
    //   );

    // Initialize the side navigation properties, and listen for
    // changes in viewport size
    this.viewportService
      .onResize()
      .pipe(startWith(this.viewportService.device))
      .subscribe(
        (device: DeviceResolution) =>
          (this.sideNavProps = this.getDashboardNavProps(device))
      );
  }

  private getDashboardNavProps(
    device: DeviceResolution
  ): DashboardSidenavProps {
    switch (device) {
      case DeviceResolution.WIDE:
      case DeviceResolution.DESKTOP:
        return new DashboardSidenavProps('side', true, false);
      case DeviceResolution.TABLET:
        return new DashboardSidenavProps('side', true, false);
      default:
        return new DashboardSidenavProps('over', false, false);
    }
  }
}
