import { DashboardMenuItem } from './dashboard-menu-item.model';

export interface DashboardStateModel {
  titleText: string;
  titleDescriptionText: string;
  menuItems: DashboardMenuItem[];
  userProfileFullNameText: string;
  userProfileCollegeNameText: string;
}
