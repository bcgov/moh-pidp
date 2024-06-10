import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivateFn, Router } from '@angular/router';

import { APP_CONFIG } from '@app/app.config';

export interface RedirectOnFeatureFlagConfigGuardRouteData {
  featureFlagName: string;
  redirectWhenFlagIs: boolean;
  redirectUrl: string;
}

export const redirectOnFeatureFlagConfigGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot,
) => {
  const router = inject(Router);
  const config = inject(APP_CONFIG);

  const data = route.data
    .redirectOnFeatureFlagConfigData as RedirectOnFeatureFlagConfigGuardRouteData;
  const featureFlags = config.featureFlags;
  const flag = featureFlags[data.featureFlagName];
  if (flag) {
    const isFlagTrue = !!flag;
    if (data.redirectWhenFlagIs == isFlagTrue) {
      // redirect
      return router.createUrlTree([data.redirectUrl ?? '/'], {
        queryParams: router.getCurrentNavigation()?.extractedUrl.queryParams,
        queryParamsHandling: 'merge',
      });
    }
  }
  return true;
};
