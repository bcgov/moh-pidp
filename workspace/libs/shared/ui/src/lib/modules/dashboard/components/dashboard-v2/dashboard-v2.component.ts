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

import { BehaviorSubject } from 'rxjs';

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

  public get menuItems(): DashboardMenuItem[] {
    return this.dashboardState.menuItems ?? [];
  }
  public get showTitle(): boolean {
    return !!this.dashboardState.titleText;
  }
  public get showTitleDescription(): boolean {
    return !!this.dashboardState.titleDescriptionText;
  }
  public showMiniMenuButton$ = new BehaviorSubject<boolean>(true);
  private isSidenavOpened = false;
  public isSidenavOpened$ = new BehaviorSubject<boolean>(this.isSidenavOpened);
  private sidenavMode: MatDrawerMode = 'over';
  public sidenavMode$ = new BehaviorSubject<MatDrawerMode>(this.sidenavMode);

  private isMenuUserProfileVisible = false;
  public isMenuUserProfileVisible$ = new BehaviorSubject<boolean>(
    this.isMenuUserProfileVisible
  );

  private isLogoutButtonVisible = false;
  public isLogoutButtonVisible$ = new BehaviorSubject(
    this.isLogoutButtonVisible
  );

  private isLogoutMenuItemVisible = false;
  public isLogoutMenuItemVisible$ = new BehaviorSubject(
    this.isLogoutMenuItemVisible
  );

  private isCollegeInfoVisible = false;
  public isCollegeInfoVisible$ = new BehaviorSubject(this.isCollegeInfoVisible);

  public constructor(private viewportService: ViewportService) {
    this.viewportService.viewportBroadcast$.subscribe((viewport) =>
      this.onViewportChange(viewport)
    );
  }
  public ngOnChanges(_: SimpleChanges): void {
    this.refresh();
  }
  private onViewportChange(viewport: PidpViewport): void {
    this.viewport = viewport;
    this.refresh();
  }
  private refresh(): void {
    switch (this.viewport) {
      case PidpViewport.xsmall:
        this.showMiniMenuButton$.next(true);
        this.showMenuUserProfile(false);
        this.showSidenav(false, 'over');
        this.showLogoutButton(false);
        this.showLogoutMenuItem(true);
        break;
      case PidpViewport.small:
        this.showMiniMenuButton$.next(true);
        this.showMenuUserProfile(false);
        this.showSidenav(false, 'over');
        this.showLogoutButton(false);
        this.showLogoutMenuItem(true);
        break;
      case PidpViewport.medium:
        this.showMiniMenuButton$.next(false);
        this.showMenuUserProfile(true);
        this.showSidenav(true, 'side');
        this.showLogoutButton(true);
        this.showLogoutMenuItem(false);
        break;
      case PidpViewport.large:
        this.showMiniMenuButton$.next(false);
        this.showMenuUserProfile(true);
        this.showSidenav(true, 'side');
        this.showLogoutButton(true);
        this.showLogoutMenuItem(false);
        break;
      default:
        throw `Dashboard v2 not implemented: ${this.viewport}`;
    }
  }
  private showSidenav(show: boolean, mode: MatDrawerMode): void {
    if (show !== this.isSidenavOpened) {
      this.isSidenavOpened = show;
      this.isSidenavOpened$.next(show);
    }
    if (mode !== this.sidenavMode) {
      this.sidenavMode = mode;
      this.sidenavMode$.next(mode);
    }
  }
  private showMenuUserProfile(show: boolean): void {
    if (
      this.dashboardState?.userProfileFullNameText &&
      show !== this.isMenuUserProfileVisible
    ) {
      this.isMenuUserProfileVisible = show;
      this.isMenuUserProfileVisible$.next(show);
    }
    if (
      this.dashboardState?.userProfileCollegeNameText &&
      show !== this.isCollegeInfoVisible
    ) {
      this.isCollegeInfoVisible = show;
      this.isCollegeInfoVisible$.next(show);
    }
  }
  private showLogoutButton(show: boolean): void {
    if (show !== this.isLogoutButtonVisible) {
      this.isLogoutButtonVisible = show;
      this.isLogoutButtonVisible$.next(show);
    }
  }
  private showLogoutMenuItem(show: boolean): void {
    if (show !== this.isLogoutMenuItemVisible) {
      this.isLogoutMenuItemVisible = show;
      this.isLogoutMenuItemVisible$.next(show);
    }
  }

  public onMiniMenuButtonClick(): void {
    // Toggle display of the sidenav.
    this.showSidenav(!this.isSidenavOpened, this.sidenavMode);
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
}
