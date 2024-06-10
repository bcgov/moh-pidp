/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import {
  ActivatedRouteSnapshot,
  Router,
  RouterStateSnapshot,
} from '@angular/router';

import { Spy, createSpyFromClass, provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import {
  RedirectOnFeatureFlagConfigGuardRouteData,
  redirectOnFeatureFlagConfigGuard,
} from './redirect-on-feature-flag.guard';

describe('redirectOnFeatureFlagConfigGuard', () => {
  let activatedRouteSnapshotSpy: Spy<ActivatedRouteSnapshot>;
  let routerStateSnapshotSpy: Spy<RouterStateSnapshot>;
  let router: Router;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        {
          provide: ActivatedRouteSnapshot,
          useValue: createSpyFromClass(ActivatedRouteSnapshot, {
            gettersToSpyOn: ['data'],
          }),
        },
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        provideAutoSpy(Router),
      ],
    });

    APP_DI_CONFIG.featureFlags = {
      feature1: true,
      feature2: false,
    };
    activatedRouteSnapshotSpy = TestBed.inject<any>(ActivatedRouteSnapshot);
    router = TestBed.inject(Router);
  });

  describe('METHOD: CanActivateFn', () => {
    given('the feature flag is set to true', () => {
      activatedRouteSnapshotSpy.accessorSpies.getters.data.mockReturnValue({
        redirectOnFeatureFlagConfigData: {
          featureFlagName: 'feature1',
          redirectWhenFlagIs: true,
          redirectUrl: '/portal',
        } as RedirectOnFeatureFlagConfigGuardRouteData,
      });

      when('the guard is called', () => {
        TestBed.runInInjectionContext(() =>
          redirectOnFeatureFlagConfigGuard(
            activatedRouteSnapshotSpy,
            routerStateSnapshotSpy,
          ),
        );
        then('the user should be redirected to the redirect URL', () => {
          expect(router.createUrlTree).toHaveBeenCalledWith(['/portal'], {
            queryParams: undefined,
            queryParamsHandling: 'merge',
          });
        });
      });
    });

    given('the feature flag is set to true, with no redirect URL', () => {
      activatedRouteSnapshotSpy.accessorSpies.getters.data.mockReturnValue({
        redirectOnFeatureFlagConfigData: {
          featureFlagName: 'feature1',
          redirectWhenFlagIs: true,
        } as RedirectOnFeatureFlagConfigGuardRouteData,
      });

      when('the guard is called', () => {
        TestBed.runInInjectionContext(() =>
          redirectOnFeatureFlagConfigGuard(
            activatedRouteSnapshotSpy,
            routerStateSnapshotSpy,
          ),
        );
        then('the user should access the route', () => {
          expect(router.createUrlTree).toHaveBeenCalledWith(['/'], {
            queryParams: undefined,
            queryParamsHandling: 'merge',
          });
        });
      });
    });

    given('the feature flag is set to false', () => {
      activatedRouteSnapshotSpy.accessorSpies.getters.data.mockReturnValue({
        redirectOnFeatureFlagConfigData: {
          featureFlagName: 'feature2',
          redirectWhenFlagIs: true,
          redirectUrl: '/portal',
        } as RedirectOnFeatureFlagConfigGuardRouteData,
      });

      when('the guard is called', () => {
        const result = TestBed.runInInjectionContext(() =>
          redirectOnFeatureFlagConfigGuard(
            activatedRouteSnapshotSpy,
            routerStateSnapshotSpy,
          ),
        );
        then('the user should access the route', () => {
          expect(result).toBeTruthy();
        });
      });
    });
  });
});
