import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  CanActivateChild,
  CanLoad,
  Route,
  Router,
  UrlTree,
} from '@angular/router';

import { Observable } from 'rxjs';

import { FeatureFlagService } from './feature-flag.service';

@Injectable({
  providedIn: 'root',
})
export class FeatureFlagGuard
  implements CanActivate, CanActivateChild, CanLoad
{
  public constructor(
    private router: Router,
    private featureFlagService: FeatureFlagService
  ) {}

  public canActivate(
    route: ActivatedRouteSnapshot
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    return this.checkFeatureFlags(route);
  }

  public canActivateChild(
    childRoute: ActivatedRouteSnapshot
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    return this.checkFeatureFlags(childRoute);
  }

  public canLoad(
    route: Route
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    return this.checkFeatureFlags(route);
  }

  private checkFeatureFlags(
    route: Route | ActivatedRouteSnapshot
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    const features = (route.data?.features ?? []) as string[];

    return this.featureFlagService.hasFlags(features)
      ? true
      : this.router.createUrlTree(['/']);
  }
}
