import { NgFor, NgSwitch, NgSwitchCase } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';
import { MatListModule } from '@angular/material/list';

import {
  DashboardMenuItem,
  DashboardRouteMenuItem,
} from '../../models/dashboard-menu-item.model';
import { DashboardRouteMenuItemComponent } from '../dashboard-route-menu-item/dashboard-route-menu-item.component';

@Component({
  selector: 'ui-dashboard-menu',
  templateUrl: './dashboard-menu.component.html',
  styleUrls: ['./dashboard-menu.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  imports: [
    MatListModule,
    NgFor,
    NgSwitch,
    NgSwitchCase,
    DashboardRouteMenuItemComponent,
  ],
})
export class DashboardMenuComponent {
  /**
   * @description
   * List of dashboard details used to populate the side navigation
   * links for routing within the application.
   */
  @Input() public menuItems?: DashboardMenuItem[];
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
   * Dashboard menu item action emitter.
   */
  @Output() public action: EventEmitter<DashboardMenuItem>;

  public constructor() {
    this.action = new EventEmitter<DashboardMenuItem>();
  }

  /**
   * @description
   * Menu item type narrowing for templates.
   */
  public isDashboardRouteMenuItem(menuItem: DashboardMenuItem): boolean {
    return menuItem instanceof DashboardRouteMenuItem;
  }

  /**
   * @description
   * Helper to assist with issues with type narrowing in templates.
   */
  public asDashboardRouteMenuItem(
    menuItem: DashboardMenuItem,
  ): DashboardRouteMenuItem {
    return menuItem as DashboardRouteMenuItem;
  }

  public onMenuItemRoute(dashboardMenuItem: DashboardMenuItem): void {
    this.action.emit(dashboardMenuItem);
  }
}
