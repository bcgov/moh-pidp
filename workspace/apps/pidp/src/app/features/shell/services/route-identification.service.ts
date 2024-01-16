import { Injectable } from '@angular/core';
import { ActivationEnd, Router } from '@angular/router';

import { BehaviorSubject } from 'rxjs';

export const knownRouteNames = {
  portal: 'portal',
  profile: {
    collegeLicenseInfo: 'college-licence-info',
  },
  access: {
    driverFitness: 'driver-fitness',
  },
};
export interface RouteChangeDetectedEventArg {
  path: string;
  routeName: string;
}
@Injectable({
  providedIn: 'root',
})
export class RouteIdentificationService {
  private defaultRouteChangeDetectedEventArg: RouteChangeDetectedEventArg = {
    path: '',
    routeName: '',
  };
  private routeNameSubject = new BehaviorSubject<RouteChangeDetectedEventArg>(
    this.defaultRouteChangeDetectedEventArg,
  );
  public routeNameBroadcast$ = this.routeNameSubject.asObservable();

  public constructor(router: Router) {
    router.events.subscribe((event) => {
      if (event instanceof ActivationEnd) {
        this.onRouteChangeDetected();
      }
    });
  }
  private onRouteChangeDetected(): void {
    const path = window.location.pathname;
    const routeName = this.getRouteName(path);
    const arg: RouteChangeDetectedEventArg = { path, routeName };
    this.routeNameSubject.next(arg);
  }
  private getRouteName(path: string): string {
    switch (path?.toLowerCase() ?? '') {
      case '/profile/college-licence-info':
        return knownRouteNames.profile.collegeLicenseInfo;
      case '/portal':
        return knownRouteNames.portal;
      case '/access/driver-fitness/v2':
        return knownRouteNames.access.driverFitness;
      default:
        return '';
    }
  }
}
