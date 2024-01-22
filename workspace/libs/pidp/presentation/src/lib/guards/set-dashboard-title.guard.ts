import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivateFn } from '@angular/router';

import { ApplicationService } from '../services/application.service';

export interface SetDashboardTitleGuardRouteData {
  titleText: string;
  titleDescriptionText: string;
}

/**
 * Use this to set the dashboard title and title description to static text
 * upon activation of any route.
 * Pass in route data named setDashboardTitleGuard that matches the
 * interface SetDashboardTitleGuardRouteData.
 */
export const setDashboardTitleGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot,
) => {
  const applicationService = inject(ApplicationService);

  const data = route.data
    .setDashboardTitleGuard as SetDashboardTitleGuardRouteData;
  if (!data) {
    throw 'setDashboardTitleGuard requires a route data item named "setDashboardTitleGuard"';
  }

  applicationService.setDashboardTitleText(
    data.titleText,
    data.titleDescriptionText,
  );

  return true;
};
