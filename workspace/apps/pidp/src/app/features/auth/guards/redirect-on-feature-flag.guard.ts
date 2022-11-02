import { Inject, Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  UrlTree,
} from '@angular/router';

import { Observable } from 'rxjs';

import { APP_CONFIG, AppConfig } from '@app/app.config';

export interface RedirectOnFeatureFlagConfigGuardRouteData {
  featureFlagName: string;
  redirectWhenFlagIs: boolean;
  redirectUrl: string;
}

@Injectable({
  providedIn: 'root',
})
export abstract class RedirectOnFeatureFlagConfigGuard implements CanActivate {
  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private router: Router
  ) {}
  public canActivate(
    route: ActivatedRouteSnapshot
  ):
    | boolean
    | UrlTree
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree> {
    const data = route.data
      .redirectOnFeatureFlagConfigData as RedirectOnFeatureFlagConfigGuardRouteData;
    const featureFlags = this.config.featureFlags;
    const flag = featureFlags[data.featureFlagName];
    if (flag) {
      const isFlagTrue = !!flag;
      if (data.redirectWhenFlagIs == isFlagTrue) {
        // redirect
        return this.router.createUrlTree([data.redirectUrl ?? '/'], {
          queryParams:
            this.router.getCurrentNavigation()?.extractedUrl.queryParams,
          queryParamsHandling: 'merge',
        });
      }
    }
    return true;
  }
}
