import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  SimpleChanges,
} from '@angular/core';
import { MatDrawerMode } from '@angular/material/sidenav';
import { IsActiveMatchOptions } from '@angular/router';

import { RoutePath } from '@bcgov/shared/utils';

import { PidpViewport, ViewportService } from '../../../../services';
import {
  DashboardMenuItem,
  DashboardRouteMenuItem,
  DashboardStateModel,
} from '../../models';

@Component({
  selector: 'ui-dashboard-v2',
  templateUrl: './dashboard-v2.component.html',
  styleUrls: ['./dashboard-v2.component.scss'],
})
export class DashboardV2Component implements OnChanges {
  @Input() public dashboardState!: DashboardStateModel;
  @Output() public logout = new EventEmitter<void>();

  private viewport = PidpViewport.xsmall;
  public showMiniMenuButton = true;
  public isSidenavOpened = false;
  public sidenavMode: MatDrawerMode = 'over';
  public isMenuUserProfileVisible = false;
  public isLogoutButtonVisible = false;
  public isLogoutMenuItemVisible = false;

  public get menuItems(): DashboardMenuItem[] {
    return this.dashboardState.menuItems ?? [];
  }
  public get showTitle(): boolean {
    return !!this.dashboardState.titleText;
  }
  public get showTitleDescription(): boolean {
    return !!this.dashboardState.titleDescriptionText;
  }
  public get isCollegeInfoVisible(): boolean {
    return !!this.dashboardState?.userProfileCollegeNameText;
  }

  public constructor(private viewportService: ViewportService) {
    this.viewportService.viewportBroadcast$.subscribe((viewport) =>
      this.onViewportChange(viewport)
    );
  }
  public ngOnChanges(_: SimpleChanges): void {
    this.refresh();
  }
  public onMiniMenuButtonClick(): void {
    // Toggle display of the sidenav.
    this.isSidenavOpened = !this.isSidenavOpened;
  }

  public getRouterLink(
    item: DashboardMenuItem | string
  ): RoutePath | undefined {
    if (item instanceof DashboardRouteMenuItem) {
      const routeItem = item as DashboardRouteMenuItem;
      return routeItem.commands;
    }
    return undefined;
  }
  public getRouterLinkActiveOptions(item: DashboardMenuItem):
    | {
        exact: boolean;
      }
    | IsActiveMatchOptions {
    if (item instanceof DashboardRouteMenuItem) {
      const routeItem = item as DashboardRouteMenuItem;
      return routeItem.linkActiveOptions;
    }
    throw 'getRouterLinkActiveOptions: not implemented';
  }
  public getRouterLinkFragment(item: DashboardMenuItem): string | undefined {
    if (item instanceof DashboardRouteMenuItem) {
      const routeItem = item as DashboardRouteMenuItem;
      return routeItem.extras?.fragment;
    }
    return undefined;
  }
  public onLogout(): void {
    this.logout.emit();
  }

  private onViewportChange(viewport: PidpViewport): void {
    this.viewport = viewport;
    this.refresh();
  }
  private refresh(): void {
    switch (this.viewport) {
      case PidpViewport.xsmall:
        this.showMiniMenuButton = true;
        this.isMenuUserProfileVisible = false;
        this.isSidenavOpened = false;
        this.sidenavMode = 'over';

        this.isLogoutButtonVisible = false;
        this.isLogoutMenuItemVisible = true;
        break;
      case PidpViewport.small:
        this.showMiniMenuButton = true;
        this.isMenuUserProfileVisible = false;
        this.isSidenavOpened = false;
        this.sidenavMode = 'over';
        this.isLogoutButtonVisible = false;
        this.isLogoutMenuItemVisible = true;
        break;
      case PidpViewport.medium:
        this.showMiniMenuButton = false;
        this.isMenuUserProfileVisible = false;
        this.isSidenavOpened = true;
        this.sidenavMode = 'side';
        this.isLogoutButtonVisible = true;
        this.isLogoutMenuItemVisible = false;
        break;
      case PidpViewport.large:
        this.showMiniMenuButton = false;
        this.isMenuUserProfileVisible = false;
        this.isSidenavOpened = true;
        this.sidenavMode = 'side';
        this.isLogoutButtonVisible = true;
        this.isLogoutMenuItemVisible = false;
        break;
      default:
        throw `Dashboard v2 not implemented: ${this.viewport}`;
    }
  }
}
