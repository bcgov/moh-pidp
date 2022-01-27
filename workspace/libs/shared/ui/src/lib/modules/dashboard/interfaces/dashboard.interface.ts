import { Observable } from 'rxjs';

import { DashboardMenuItem } from '../models/dashboard-menu-item.model';

export interface IDashboard {
  /**
   * @description
   * Modules specific logout URL.
   */
  logoutRedirectUrl: string;
  /**
   * @description
   * Logout event handler.
   */
  onLogout: () => void;
}
