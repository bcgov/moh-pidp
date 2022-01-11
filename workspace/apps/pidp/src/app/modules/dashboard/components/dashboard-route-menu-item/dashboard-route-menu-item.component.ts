import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';

// TODO get rid of this dependency and make it an input binding
import { ViewportService } from '@core/services/viewport.service';

import { DashboardRouteMenuItem } from '../../models/dashboard-menu-item.model';

@Component({
  selector: 'app-dashboard-route-menu-item',
  templateUrl: './dashboard-route-menu-item.component.html',
  styleUrls: ['./dashboard-route-menu-item.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DashboardRouteMenuItemComponent {
  /**
   * @description
   * Current route menu item.
   */
  @Input() public routeMenuItem!: DashboardRouteMenuItem;
  /**
   * @description
   * Whether the dashboard menu items are responsive, and collapse
   * on mobile viewports.
   */
  @Input() public responsiveMenuItems!: boolean;
  /**
   * @description
   * Whether the dashboard menu items should display their icons.
   */
  @Input() public showMenuItemIcons!: boolean;
  /**
   * @description
   * Dashboard menu item route emitter.
   */
  @Output() public route: EventEmitter<DashboardRouteMenuItem>;

  public constructor(private viewportService: ViewportService) {
    this.route = new EventEmitter<DashboardRouteMenuItem>();
  }

  public get isMobile(): boolean {
    return this.viewportService.isMobile;
  }

  public get isDesktop(): boolean {
    return this.viewportService.isDesktop || this.viewportService.isWideDesktop;
  }

  public onRoute(routeMenuItem: DashboardRouteMenuItem): void {
    this.route.emit(routeMenuItem);
  }
}
