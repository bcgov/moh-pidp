/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import {
  ActivatedRouteSnapshot,
  Route,
  RouterStateSnapshot,
  UrlSegment,
} from '@angular/router';

import { Spy, provideAutoSpy } from 'jest-auto-spies';

import { AuthenticationGuard } from './authentication.guard';
import { AuthenticationGuardService } from './services/authentication-guard.service';

describe('AuthenticationGuard', () => {
  let activatedRouteSnapshotSpy: Spy<ActivatedRouteSnapshot>;
  let routerStateSnapshotSpy: Spy<RouterStateSnapshot>;

  let authenticationGuardServiceSpy: Spy<AuthenticationGuardService>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideAutoSpy(AuthenticationGuardService)],
    });

    authenticationGuardServiceSpy = TestBed.inject<any>(
      AuthenticationGuardService,
    );
    activatedRouteSnapshotSpy = TestBed.inject<any>(ActivatedRouteSnapshot);
    routerStateSnapshotSpy = TestBed.inject<any>(RouterStateSnapshot);
  });

  describe('METHOD: CanActivateFn', () => {
    given('the user is authorized', () => {
      authenticationGuardServiceSpy.canActivate.mockReturnValueOnce(true);
      when('the guard is called', () => {
        const result = TestBed.runInInjectionContext(() =>
          AuthenticationGuard.canActivate(
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

  describe('METHOD: canActivateChild', () => {
    given('the user is authorized', () => {
      authenticationGuardServiceSpy.canActivateChild.mockReturnValueOnce(true);

      when('the guard is called', () => {
        const result = TestBed.runInInjectionContext(() =>
          AuthenticationGuard.canActivateChild(
            activatedRouteSnapshotSpy,
            routerStateSnapshotSpy,
          ),
        );
        then('the user should access the child route', () => {
          expect(result).toBeTruthy();
        });
      });
    });
  });

  describe('METHOD: CanMatchFn', () => {
    let route: Route;
    let urlSegment: UrlSegment[];
    given('the user is authorized', () => {
      route = {};
      urlSegment = [];
      authenticationGuardServiceSpy.canMatch.mockReturnValueOnce(true);

      when('the guard is called', () => {
        const result = TestBed.runInInjectionContext(() =>
          AuthenticationGuard.canMatch(route, urlSegment),
        );
        then('the user should access the route', () => {
          expect(result).toBeTruthy();
        });
      });
    });
  });
});
