import { NgClass, NgFor, NgIf, NgTemplateOutlet } from '@angular/common';
import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
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

import { Observable, Subject, takeUntil } from 'rxjs';

import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faBell } from '@fortawesome/free-regular-svg-icons';

import {
  DashboardMenuItem,
  DashboardRouteMenuItem,
  InjectViewportCssClassDirective,
  LayoutHeaderFooterComponent,
  PidpViewport,
  ViewportService,
} from '@bcgov/shared/ui';
import { RoutePath } from '@bcgov/shared/utils';

import { PartyService } from '@app/core/party/party.service';
import { AccessRoutes } from '@app/features/access/access.routes';
import { IdentityProvider } from '@app/features/auth/enums/identity-provider.enum';
import { AlertCode } from '@app/features/portal/enums/alert-code.enum';
import { ProfileRoutes } from '@app/features/profile/profile.routes';

import { Credential } from './nav-menu.model';
import { NavMenuResource } from './nav-menu.resource.service';

@Component({
  selector: 'app-nav-menu',
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
    FaIconComponent,
    NgClass,
  ],
})
export class NavMenuComponent implements OnChanges, OnInit, OnDestroy {
  @Input() public alerts: AlertCode[] | null = [];
  @Input() public menuItems!: DashboardMenuItem[];
  @Input() public emailSupport!: string;
  @Input() public collegeRoute!: string;
  @Output() public logout = new EventEmitter<void>();
  @ViewChild('sidenav') public sidenav!: MatSidenav;

  private viewport = PidpViewport.xsmall;
  public showMiniMenuButton = true;
  public isSidenavOpened = false;
  public sidenavMode: MatDrawerMode = 'over';

  public isLogoutButtonVisible = false;
  public isLogoutMenuItemVisible = false;
  public isTopMenuVisible = false;
  public ProfileRoutes = ProfileRoutes;
  public AccessRoutes = AccessRoutes;
  public showCollegeAlert = false;
  public faBell = faBell;
  public AlertCode = AlertCode;
  public credentials: Credential[] = [];
  public credentials$: Observable<Credential[]>;
  private unsubscribe$ = new Subject<void>();
  public IdentityProvider = IdentityProvider;

  public constructor(
    private viewportService: ViewportService,
    private router: Router,
    private resource: NavMenuResource,
    private partyService: PartyService,
  ) {
    this.viewportService.viewportBroadcast$.subscribe((viewport) =>
      this.onViewportChange(viewport),
    );
    const partyId = this.partyService.partyId;
    this.credentials$ = this.resource.getCredentials(partyId);
  }

  public ngOnInit(): void {
    this.handleLinkedAccounts();
  }

  public ngOnChanges(_: SimpleChanges): void {
    this.refresh();
  }

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  public onMiniMenuButtonClick(): void {
    // Toggle display of the sidenav.
    this.isSidenavOpened = !this.isSidenavOpened;
  }
  public navigateToRoot(): void {
    this.router.navigateByUrl('/');
  }

  public navigateTo(route: string): void {
    if (this.showMiniMenuButton)
      this.isSidenavOpened = this.isSidenavOpened
        ? !this.isSidenavOpened
        : this.isSidenavOpened;
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

  public hasCredential(idp: IdentityProvider): boolean {
    return this.credentials.some((c) => c.identityProvider === idp);
  }

  public onLogout(): void {
    this.logout.emit();
  }
  public onMenuItemClicked(): void {
    if (this.showMiniMenuButton && this.sidenavMode === 'over') {
      this.sidenav.close();
    }
  }

  private handleLinkedAccounts(): void {
    this.credentials$
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((credentials) => {
        this.credentials = credentials;
      });
  }

  private onViewportChange(viewport: PidpViewport): void {
    this.viewport = viewport;
    this.refresh();
  }
  private refresh(): void {
    if (
      ![
        PidpViewport.xsmall,
        PidpViewport.small,
        PidpViewport.medium,
        PidpViewport.large,
      ].includes(this.viewport)
    ) {
      throw new Error(`Nav Menu not implemented: ${this.viewport}`);
    }

    const isMiniView = this.viewport === PidpViewport.xsmall;
    const isDesktopView =
      this.viewport === PidpViewport.medium ||
      this.viewport === PidpViewport.large;

    this.showMiniMenuButton = isMiniView;
    this.isSidenavOpened = isDesktopView;
    this.sidenavMode = isDesktopView ? 'side' : 'over';
    this.isLogoutButtonVisible = !isMiniView;
    this.isLogoutMenuItemVisible = !isDesktopView;
    this.isTopMenuVisible = !isMiniView;
  }
}
