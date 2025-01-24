/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import {
  ActivatedRouteSnapshot,
  Router,
  RouterStateSnapshot,
} from '@angular/router';

import { Spy, createSpyFromClass, provideAutoSpy } from 'jest-auto-spies';

import { AuthorizedUserService } from '@app/features/auth/services/authorized-user.service';
import { Role } from '@app/shared/enums/roles.enum';

import { adminGuard } from './admin.guard';

describe('adminGuard', () => {
  let activatedRouteSnapshotSpy: Spy<ActivatedRouteSnapshot>;
  let routerStateSnapshotSpy: Spy<RouterStateSnapshot>;
  let authorizedUserServiceSpy: Spy<AuthorizedUserService>;
  let router: Router;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        {
          provide: AuthorizedUserService,
          useValue: createSpyFromClass(AuthorizedUserService, {
            gettersToSpyOn: ['roles'],
          }),
        },
        provideAutoSpy(Router),
        provideAutoSpy(ActivatedRouteSnapshot),
        provideAutoSpy(RouterStateSnapshot),
      ],
    });
    activatedRouteSnapshotSpy = TestBed.inject<any>(ActivatedRouteSnapshot);
    routerStateSnapshotSpy = TestBed.inject<any>(RouterStateSnapshot);
    authorizedUserServiceSpy = TestBed.inject<any>(AuthorizedUserService);
    router = TestBed.inject(Router);
  });

  describe('METHOD: CanActivateFn', () => {
    given('the user has no role', () => {
      authorizedUserServiceSpy.accessorSpies.getters.roles.mockReturnValue([]);

      when('the guard is called', () => {
        TestBed.runInInjectionContext(() =>
          adminGuard(activatedRouteSnapshotSpy, routerStateSnapshotSpy),
        );

        then('the user should be redirected to the root route', () => {
          expect(router.createUrlTree).toHaveBeenCalledWith(['/']);
        });
      });
    });

    given('the user has the role FEATURE_PIDP_DEMO', () => {
      authorizedUserServiceSpy.accessorSpies.getters.roles.mockReturnValue([
        Role.FEATURE_PIDP_DEMO,
      ]);

      when('the guard is called', () => {
        TestBed.runInInjectionContext(() =>
          adminGuard(activatedRouteSnapshotSpy, routerStateSnapshotSpy),
        );

        then('the user should be redirected to the root route', () => {
          expect(router.createUrlTree).toHaveBeenCalledWith(['/']);
        });
      });
    });

    given('the user has the ADMIN role', () => {
      authorizedUserServiceSpy.accessorSpies.getters.roles.mockReturnValue([
        Role.ADMIN,
      ]);
      when('the guard is called', () => {
        const result = TestBed.runInInjectionContext(() =>
          adminGuard(activatedRouteSnapshotSpy, routerStateSnapshotSpy),
        );
        then('the user should access the route', () => {
          expect(result).toBeTruthy();
        });
      });
    });
  });
});
