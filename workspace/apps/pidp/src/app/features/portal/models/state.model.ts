import { AlertCode } from '../enums/alert-code.enum';

export const PidpStateName = {
  dashboard: 'dashboard',
};
export interface NamedState {
  stateName: string;
}
export interface DashboardStateModel extends NamedState {
  titleText: string;
  titleDescriptionText: string;
  userProfileFullNameText: string;
  userProfileCollegeNameText: string;
  collegeRoute: string;
  alerts: AlertCode[];
}
export const defaultDashboardState: DashboardStateModel = {
  stateName: PidpStateName.dashboard,
  titleText: '',
  titleDescriptionText: '',
  userProfileFullNameText: '',
  userProfileCollegeNameText: '',
  collegeRoute: '',
  alerts: [],
};
export interface ApplicationStateModel {
  all: NamedState[];
}
export const defaultApplicationState: ApplicationStateModel = {
  all: [{ ...defaultDashboardState }],
};
