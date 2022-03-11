import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';

import { Observable, combineLatest, distinctUntilChanged, map } from 'rxjs';

import { ViewportService } from '../../../../services/viewport.service';
import { DashboardHeaderTheme } from '../../models/dashboard-header-config.model';

@Component({
  selector: 'ui-dashboard-header',
  templateUrl: './dashboard-header.component.html',
  styleUrls: ['./dashboard-header.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DashboardHeaderComponent {
  /**
   * @description
   * Theme for the dashboard header.
   */
  @Input() public theme: DashboardHeaderTheme;
  /**
   * @description
   * Allow the mobile toggle to be displayed.
   */
  @Input() public allowMobileToggle?: boolean;
  /**
   * @description
   * Username displayed in the dashboard header.
   */
  @Input() public username?: string;
  /**
   * @description
   * Event emission of mobile menu action.
   */
  @Output() public toggleMobileMenu: EventEmitter<void>;
  /**
   * @description
   * Event emission of logout action.
   */
  @Output() public logout: EventEmitter<void>;

  public mobileToggleBreakpoint$: Observable<boolean>;
  public usernameBreakpoint$: Observable<boolean>;

  public constructor(public viewportService: ViewportService) {
    this.theme = 'dark';
    this.toggleMobileMenu = new EventEmitter<void>();
    this.logout = new EventEmitter<void>();

    this.mobileToggleBreakpoint$ = combineLatest([
      viewportService.isMobileBreakpoint$,
      viewportService.isTabletBreakpoint$,
    ]).pipe(
      map(([isMobile, isTablet]: [boolean, boolean]) => isMobile || isTablet),
      distinctUntilChanged()
    );
    this.usernameBreakpoint$ = viewportService.isTabletAndUpBreakpoint$;
  }

  public onOpenMenu(): void {
    this.toggleMobileMenu.emit();
  }

  public onLogout(): void {
    this.logout.emit();
  }
}
