/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import {
  ActivatedRouteSnapshot,
  Router,
  RouterStateSnapshot,
} from '@angular/router';

import { highAssuranceCredentialGuard } from './high-assurance-credential.guard';
import { Spy, createSpyFromClass, provideAutoSpy } from 'jest-auto-spies';
import { AuthorizedUserService } from '../services/authorized-user.service';
import { IdentityProvider } from '../enums/identity-provider.enum';
import { KeycloakService } from 'keycloak-angular';
import { of } from 'rxjs';

describe('highAssuranceCredentialGuard', () => {
  let activatedRouteSnapshotSpy: Spy<ActivatedRouteSnapshot>;
  let routerStateSnapshotSpy: Spy<RouterStateSnapshot>;
  let authorizedUserServiceSpy: Spy<AuthorizedUserService>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        // provideAutoSpy(AuthorizedUserService),
        {
          provide: AuthorizedUserService,
          useValue: createSpyFromClass(AuthorizedUserService, {
            gettersToSpyOn: ['identityProvider$'],
            settersToSpyOn: ['identityProvider$'],
          }),
        },
        provideAutoSpy(KeycloakService),
        provideAutoSpy(Router),
      ],
    });

    authorizedUserServiceSpy = TestBed.inject<any>(AuthorizedUserService);
  });

  describe('METHOD: CanActivateFn', () => {
    given('the user is authorized', () => {
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
          expect(result).toBeTruthy();
        });
      });
    });

    given('the user is authorized', () => {
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
          expect(result).toBeTruthy();
        });
      });
    });
  });
});
