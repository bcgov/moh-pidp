import { AsyncPipe, NgIf } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { RouterLink, RouterLinkActive } from '@angular/router';

import { Observable } from 'rxjs';

import { ViewportService } from '../../../../services/viewport.service';
import { DashboardRouteMenuItem } from '../../models/dashboard-menu-item.model';

@Component({
  selector: 'ui-dashboard-route-menu-item',
  templateUrl: './dashboard-route-menu-item.component.html',
  styleUrls: ['./dashboard-route-menu-item.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  imports: [
    MatListModule,
    RouterLinkActive,
    RouterLink,
    NgIf,
    MatIconModule,
    AsyncPipe,
  ],
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
   * down to icons when on mobile viewports.
   */
  @Input() public responsiveMenuItems?: boolean;
  /**
   * @description
   * Whether the dashboard menu items should display their icons.
   */
  @Input() public showMenuItemIcons?: boolean;
  /**
   * @description
   * Dashboard menu item emission of route action.
   */
  @Output() public route: EventEmitter<DashboardRouteMenuItem>;

  public isDesktopAndUpBreakpoint$: Observable<boolean>;

  public constructor(viewportService: ViewportService) {
    this.route = new EventEmitter<DashboardRouteMenuItem>();
    this.isDesktopAndUpBreakpoint$ = viewportService.isDesktopAndUpBreakpoint$;
  }

  public onRoute(routeMenuItem: DashboardRouteMenuItem): void {
    this.route.emit(routeMenuItem);
  }
}
