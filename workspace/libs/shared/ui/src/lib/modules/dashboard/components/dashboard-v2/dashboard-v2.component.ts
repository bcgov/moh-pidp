import { NgFor, NgIf, NgTemplateOutlet } from '@angular/common';
import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import {
  MatDrawerMode,
  MatSidenav,
  MatSidenavModule,
} from '@angular/material/sidenav';
import { MatTooltipModule } from '@angular/material/tooltip';
import {
  IsActiveMatchOptions,
  RouterLink,
  RouterLinkActive,
  RouterOutlet,
} from '@angular/router';

import { DashboardStateModel } from '@pidp/data-model';

import { RoutePath } from '@bcgov/shared/utils';

import { LayoutHeaderFooterComponent } from '../../../../components/layout-header-footer/layout-header-footer.component';
import { InjectViewportCssClassDirective } from '../../../../directives/viewport-css.directive';
import { PidpViewport, ViewportService } from '../../../../services';
import { DashboardMenuItem, DashboardRouteMenuItem } from '../../models';

@Component({
  selector: 'ui-dashboard-v2',
  templateUrl: './dashboard-v2.component.html',
  styleUrls: ['./dashboard-v2.component.scss'],
  standalone: true,
  imports: [
    InjectViewportCssClassDirective,
    LayoutHeaderFooterComponent,
    MatButtonModule,
    MatIconModule,
    MatSidenavModule,
    MatTooltipModule,
    NgFor,
    NgIf,
    NgTemplateOutlet,
    RouterLink,
    RouterLinkActive,
    RouterOutlet,
  ],
})
export class DashboardV2Component implements OnChanges {
  @Input() public dashboardState!: DashboardStateModel;
  @Input() public menuItems!: DashboardMenuItem[];
  @Input() public emailSupport!: string;
  @Output() public logout = new EventEmitter<void>();
  @ViewChild('sidenav') public sidenav!: MatSidenav;

  private viewport = PidpViewport.xsmall;
  public showMiniMenuButton = true;
  public isSidenavOpened = false;
  public sidenavMode: MatDrawerMode = 'over';
  public get isHeaderImageVisible(): boolean {
    // Hide the image in xsmall small when there is title text to display.
    if (
      (this.viewport === PidpViewport.small ||
        this.viewport === PidpViewport.xsmall) &&
      this.dashboardState.titleText
    ) {
      return false;
    }
    return true;
  }

  public get isMenuUserProfileVisible(): boolean {
    // NOTE: The user name section is always hidden in xsmall and small views
    if (
      this.viewport === PidpViewport.small ||
      this.viewport === PidpViewport.xsmall
    ) {
      return false;
    }
    return !!this.dashboardState.userProfileFullNameText;
  }
  public isLogoutButtonVisible = false;
  public isLogoutMenuItemVisible = false;

  public get showTitle(): boolean {
    return !!this.dashboardState.titleText;
  }
  public get showTitleDescription(): boolean {
    return !!this.dashboardState.titleDescriptionText;
  }
  public get isCollegeInfoVisible(): boolean {
    return (
      this.isMenuUserProfileVisible &&
      !!this.dashboardState.userProfileCollegeNameText
    );
  }

  public constructor(private viewportService: ViewportService) {
    this.viewportService.viewportBroadcast$.subscribe((viewport) =>
      this.onViewportChange(viewport),
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
    item: DashboardMenuItem | string,
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
  public onMenuItemClicked(): void {
    if (this.sidenavMode === 'over') {
      this.sidenav.close();
    }
  }

  private onViewportChange(viewport: PidpViewport): void {
    this.viewport = viewport;
    this.refresh();
  }
  private refresh(): void {
    switch (this.viewport) {
      case PidpViewport.xsmall:
        this.showMiniMenuButton = true;
        this.isSidenavOpened = false;
        this.sidenavMode = 'over';

        this.isLogoutButtonVisible = false;
        this.isLogoutMenuItemVisible = true;
        break;
      case PidpViewport.small:
        this.showMiniMenuButton = true;
        this.isSidenavOpened = false;
        this.sidenavMode = 'over';
        this.isLogoutButtonVisible = false;
        this.isLogoutMenuItemVisible = true;
        break;
      case PidpViewport.medium:
        this.showMiniMenuButton = false;
        this.isSidenavOpened = true;
        this.sidenavMode = 'side';
        this.isLogoutButtonVisible = true;
        this.isLogoutMenuItemVisible = false;
        break;
      case PidpViewport.large:
        this.showMiniMenuButton = false;
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
