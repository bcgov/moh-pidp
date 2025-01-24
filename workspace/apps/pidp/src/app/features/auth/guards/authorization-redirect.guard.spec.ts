/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import { ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

import { Spy, provideAutoSpy } from 'jest-auto-spies';

import { authorizationRedirectGuard } from './authorization-redirect.guard';
import { AuthorizationRedirectGuardService } from './services/authorization-redirect-guard.service';

describe('authorizationRedirectGuard', () => {
  let activatedRouteSnapshotSpy: Spy<ActivatedRouteSnapshot>;
  let routerStateSnapshotSpy: Spy<RouterStateSnapshot>;
  let authorizationRedirectGuardServiceSpy: Spy<AuthorizationRedirectGuardService>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideAutoSpy(AuthorizationRedirectGuardService)],
    });

    authorizationRedirectGuardServiceSpy = TestBed.inject<any>(
      AuthorizationRedirectGuardService,
    );
    activatedRouteSnapshotSpy = TestBed.inject<any>(ActivatedRouteSnapshot);
    routerStateSnapshotSpy = TestBed.inject<any>(RouterStateSnapshot);
  });

  describe('METHOD: CanActivateFn', () => {
    given('the user is authorized', () => {
      authorizationRedirectGuardServiceSpy.canActivate.mockReturnValueOnce(
        true,
      );

      when('the guard is called', () => {
        const result = TestBed.runInInjectionContext(() =>
          authorizationRedirectGuard(
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
