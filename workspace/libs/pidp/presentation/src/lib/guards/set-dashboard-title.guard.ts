import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, UrlTree } from '@angular/router';

import { Observable } from 'rxjs';

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
@Injectable({ providedIn: 'root' })
export class SetDashboardTitleGuard implements CanActivate {
  public constructor(private applicationService: ApplicationService) {}
  public canActivate(
    route: ActivatedRouteSnapshot
  ):
    | boolean
    | UrlTree
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree> {
    const data = route.data
      .setDashboardTitleGuard as SetDashboardTitleGuardRouteData;
    if (!data) {
      throw 'SetDashboardTitleGuard requires a route data item named "setDashboardTitleGuard"';
    }

    this.applicationService.setDashboardTitleText(
      data.titleText,
      data.titleDescriptionText
    );

    return true;
  }
}
