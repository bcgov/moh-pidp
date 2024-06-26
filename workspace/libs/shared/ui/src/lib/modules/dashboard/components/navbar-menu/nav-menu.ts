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
  Router,
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
  selector: 'ui-nav-menu',
  templateUrl: './nav-menu.html',
  styleUrls: ['./nav-menu.scss'],
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
export class NavMenuComponent implements OnChanges {
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

  public isLogoutButtonVisible = false;
  public isLogoutMenuItemVisible = false;
  public isTopMenuVisible = false;

  public get showTitle(): boolean {
    return !!this.dashboardState.titleText;
  }
  public get showTitleDescription(): boolean {
    return !!this.dashboardState.titleDescriptionText;
  }

  public get collegeRoute(): boolean {
    return !!this.dashboardState.collegeRoute;
  }

  public constructor(
    private viewportService: ViewportService,
    private router: Router,
  ) {
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
  public navigateToRoot(): void {
    this.router.navigateByUrl('/');
  }

  public navigateTo(route: string): void {
    if(this.showMiniMenuButton)
      this.isSidenavOpened = this.isSidenavOpened ? !this.isSidenavOpened : this.isSidenavOpened;
    this.router.navigateByUrl(route);
  }

  public getRouterLink(
    item: DashboardMenuItem | string,
  ): RoutePath | undefined {
    if (item instanceof DashboardRouteMenuItem) {
      const routeItem: DashboardRouteMenuItem = item;
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
      const routeItem: DashboardRouteMenuItem = item;
      return routeItem.linkActiveOptions;
    }
    throw new Error('getRouterLinkActiveOptions: not implemented');
  }
  public getRouterLinkFragment(item: DashboardMenuItem): string | undefined {
    if (item instanceof DashboardRouteMenuItem) {
      const routeItem: DashboardRouteMenuItem = item;
      return routeItem.extras?.fragment;
    }
    return undefined;
  }
  public onLogout(): void {
    this.logout.emit();
  }
  public onMenuItemClicked(): void {
    if (this.showMiniMenuButton && this.sidenavMode === 'over') {
      this.sidenav.close();
    }
  }

  private onViewportChange(viewport: PidpViewport): void {
    this.viewport = viewport;
    this.refresh();
  }
  private refresh(): void {
    if(!([PidpViewport.xsmall, PidpViewport.small, PidpViewport.medium, PidpViewport.large].includes(this.viewport))) {
      throw new Error(`Nav Menu not implemented: ${this.viewport}`);
    }


    const isMiniView = this.viewport === PidpViewport.xsmall;
    const isDesktopView = this.viewport === PidpViewport.medium || this.viewport === PidpViewport.large;

    this.showMiniMenuButton = isMiniView;
    this.isSidenavOpened = isDesktopView;
    this.sidenavMode = isDesktopView ? 'side' : 'over';
    this.isLogoutButtonVisible = !isMiniView;
    this.isLogoutMenuItemVisible = !isDesktopView;
    this.isTopMenuVisible = !isMiniView;
  }
}
