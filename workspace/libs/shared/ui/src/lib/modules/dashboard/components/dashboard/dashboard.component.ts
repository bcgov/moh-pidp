import { BreakpointState } from '@angular/cdk/layout';
import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';
import { MatSidenav, MatSidenavModule } from '@angular/material/sidenav';
import { RouterOutlet } from '@angular/router';

import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

import {
  BootstrapBreakpoints,
  ViewportService,
} from '../../../../services/viewport.service';
import { DashboardHeaderConfig } from '../../models/dashboard-header-config.model';
import {
  DashboardMenuItem,
  DashboardRouteMenuItem,
} from '../../models/dashboard-menu-item.model';
import { DashboardSidenavProps } from '../../models/dashboard-sidenav-props.model';
import { DashboardHeaderComponent } from '../dashboard-header/dashboard-header.component';
import { DashboardMenuComponent } from '../dashboard-menu/dashboard-menu.component';

@UntilDestroy()
@Component({
  selector: 'ui-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  imports: [
    DashboardHeaderComponent,
    MatSidenavModule,
    DashboardMenuComponent,
    RouterOutlet,
  ],
})
export class DashboardComponent implements OnInit {
  /**
   * @description
   * Dashboard header configuration.
   */
  @Input() public headerConfig: DashboardHeaderConfig;
  /**
   * @description
   * Username for the authenticated user.
   */
  @Input() public username?: string;
  /**
   * @description
   * List of dashboard details used to populate the side navigation
   * links for routing within the application.
   */
  @Input() public menuItems: DashboardMenuItem[];
  /**
   * @description
   * Whether the dashboard menu items should display their icons.
   */
  @Input() public showMenuItemIcons: boolean;
  /**
   * @description
   * Whether the dashboard menu items are responsive, and collapse
   * on mobile viewports.
   */
  @Input() public responsiveMenuItems: boolean;
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
  /**
   * @description
   * Logout event handler.
   */
  @Output() public logout: EventEmitter<void>;

  public sideNavProps!: DashboardSidenavProps;

  public constructor(private viewportService: ViewportService) {
    this.headerConfig = {
      theme: 'dark',
      allowMobileToggle: true,
    };
    this.menuItems = [];
    this.showMenuItemIcons = false;
    this.responsiveMenuItems = false;
    this.logout = new EventEmitter<void>();
  }

  /**
   * @description
   * Handle menu item actions.
   */
  public onMenuItemAction(dashboardMenuItem: DashboardMenuItem): void {
    if (
      dashboardMenuItem instanceof DashboardRouteMenuItem &&
      this.viewportService.isMobileBreakpoint
    ) {
      // Close on mobile to prevent blocking the screen when routing
      this.sideNav.close();
    }
  }

  public onLogout(): void {
    this.logout.emit();
  }

  public ngOnInit(): void {
    if (!this.showMenuItemIcons) {
      // Cannot be responsive when icons are not being shown
      this.responsiveMenuItems = false;
    }

    this.viewportService.breakpointObserver$
      .pipe(untilDestroyed(this))
      .subscribe(
        (result: BreakpointState) =>
          (this.sideNavProps = this.getSideNavProperties(result)),
      );
  }

  private getSideNavProperties(result: BreakpointState): DashboardSidenavProps {
    if (result.breakpoints[BootstrapBreakpoints.large]) {
      return new DashboardSidenavProps('side', true, false);
    } else if (result.breakpoints[BootstrapBreakpoints.medium]) {
      return new DashboardSidenavProps('side', true, false);
    } else {
      return new DashboardSidenavProps('over', false, false);
    }
  }
}
