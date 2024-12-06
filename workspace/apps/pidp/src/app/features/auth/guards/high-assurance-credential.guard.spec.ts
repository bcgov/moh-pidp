/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import {
  ActivatedRouteSnapshot,
  Router,
  RouterStateSnapshot,
} from '@angular/router';

import { Observable, of } from 'rxjs';

import { Spy, createSpyFromClass, provideAutoSpy } from 'jest-auto-spies';

import { IdentityProvider } from '../enums/identity-provider.enum';
import { AuthorizedUserService } from '../services/authorized-user.service';
import { highAssuranceCredentialGuard } from './high-assurance-credential.guard';

describe('highAssuranceCredentialGuard', () => {
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
            gettersToSpyOn: ['identityProvider$'],
          }),
        },
        provideAutoSpy(Router),
      ],
    });

    authorizedUserServiceSpy = TestBed.inject<any>(AuthorizedUserService);
    router = TestBed.inject(Router);
  });

  describe('METHOD: CanActivateFn', () => {
    given('the user is authenticated with a BC Services Card', (done) => {
      authorizedUserServiceSpy.accessorSpies.getters.identityProvider$.mockReturnValue(
        of(IdentityProvider.BCSC),
      );

      when('the guard is called', () => {
        const result = TestBed.runInInjectionContext(() =>
          highAssuranceCredentialGuard(
            activatedRouteSnapshotSpy,
            routerStateSnapshotSpy,
          ),
        );

        then('the user should access the route', () => {
          if (result instanceof Observable) {
            result.subscribe((value) => {
              try {
                expect(value).toBeTruthy();
                done();
              } catch (error: any) {
                done(error);
              }
            });
          }
        });
      });
    });

    given('the user is authenticated with a BCProvider', (done) => {
      authorizedUserServiceSpy.accessorSpies.getters.identityProvider$.mockReturnValue(
        of(IdentityProvider.BC_PROVIDER),
      );

      when('the guard is called', () => {
        const result = TestBed.runInInjectionContext(() =>
          highAssuranceCredentialGuard(
            activatedRouteSnapshotSpy,
            routerStateSnapshotSpy,
          ),
        );

        then('the user should access the route', () => {
          if (result instanceof Observable) {
            result.subscribe((value) => {
              try {
                expect(value).toBeTruthy();
                done();
              } catch (error: any) {
                done(error);
              }
            });
          }
        });
      });
    });

    given('the user is authenticated with IDIR', (done) => {
      authorizedUserServiceSpy.accessorSpies.getters.identityProvider$.mockReturnValue(
        of(IdentityProvider.IDIR),
      );

      when('the guard is called', () => {
        const result = TestBed.runInInjectionContext(() =>
          highAssuranceCredentialGuard(
            activatedRouteSnapshotSpy,
            routerStateSnapshotSpy,
          ),
        );

        then('the user should be redirect to the root URL', () => {
          if (result instanceof Observable) {
            result.subscribe(() => {
              try {
                expect(router.createUrlTree).toHaveBeenCalledWith(['/']);
                done();
              } catch (error: any) {
                done(error);
              }
            });
          }
        });
      });
    });
  });
});
