/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import {
  ActivatedRouteSnapshot,
  Router,
  RouterStateSnapshot,
} from '@angular/router';

import { Observable, of } from 'rxjs';

import { randNumber } from '@ngneat/falso';
import { Spy, createSpyFromClass, provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import {
  Destination,
  DiscoveryResource,
} from '@app/core/party/discovery-resource.service';
import { PartyService } from '@app/core/party/party.service';

import { wizardResolver } from './wizard.resolver';

describe('wizardResolver', () => {
  let activatedRouteSnapshotSpy: Spy<ActivatedRouteSnapshot>;
  let routerStateSnapshotSpy: Spy<RouterStateSnapshot>;
  let partyServiceSpy: Spy<PartyService>;
  let discoveryResource: Spy<DiscoveryResource>;
  let router: Router;

  let mockDestination: Destination;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        {
          provide: PartyService,
          useValue: createSpyFromClass(PartyService, {
            gettersToSpyOn: ['partyId'],
          }),
        },
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        provideAutoSpy(DiscoveryResource),
        provideAutoSpy(Router),
      ],
    });
    activatedRouteSnapshotSpy = TestBed.inject<any>(ActivatedRouteSnapshot);
    routerStateSnapshotSpy = TestBed.inject<any>(RouterStateSnapshot);
    partyServiceSpy = TestBed.inject(PartyService) as Spy<PartyService>;
    discoveryResource = TestBed.inject(
      DiscoveryResource,
    ) as Spy<DiscoveryResource>;
    router = TestBed.inject(Router);
  });

  given('partyId exists, destination is DEMOGRAPHICS', (done) => {
    const partyId = randNumber({ min: 1 });
    partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
    mockDestination = 1;
    discoveryResource.getDestination.mockReturnValueOnce(of(mockDestination));

    when('Resolver is called', () => {
      const result = TestBed.runInInjectionContext(() =>
        wizardResolver(activatedRouteSnapshotSpy, routerStateSnapshotSpy),
      );

      then('should navigate to portal page', () => {
        if (result instanceof Observable) {
          result.subscribe(() => {
            try {
              expect(router.navigateByUrl).toHaveBeenCalledWith('portal');
              done();
            } catch (error: any) {
              done(error);
            }
          });
        }
      });
    });
  });

  given('partyId exists, destination is USER_ACCESS_AGREEMENT', (done) => {
    const partyId = randNumber({ min: 1 });
    partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
    mockDestination = 2;
    discoveryResource.getDestination.mockReturnValueOnce(of(mockDestination));

    when('Resolver is called', () => {
      const result = TestBed.runInInjectionContext(() =>
        wizardResolver(activatedRouteSnapshotSpy, routerStateSnapshotSpy),
      );

      then('should navigate to portal page', () => {
        if (result instanceof Observable) {
          result.subscribe(() => {
            try {
              expect(router.navigateByUrl).toHaveBeenCalledWith('portal');
              done();
            } catch (error: any) {
              done(error);
            }
          });
        }
      });
    });
  });

  given('partyId exists, destination is LICENCE_DECLARATION', (done) => {
    const partyId = randNumber({ min: 1 });
    partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
    mockDestination = 3;
    discoveryResource.getDestination.mockReturnValueOnce(of(mockDestination));

    when('Resolver is called', () => {
      const result = TestBed.runInInjectionContext(() =>
        wizardResolver(activatedRouteSnapshotSpy, routerStateSnapshotSpy),
      );

      then('should navigate to portal page', () => {
        if (result instanceof Observable) {
          result.subscribe(() => {
            try {
              expect(router.navigateByUrl).toHaveBeenCalledWith('portal');
              done();
            } catch (error: any) {
              done(error);
            }
          });
        }
      });
    });
  });

  given('partyId exists, destination is PORTAL', (done) => {
    const partyId = randNumber({ min: 1 });
    partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
    mockDestination = 4;
    discoveryResource.getDestination.mockReturnValueOnce(of(mockDestination));

    when('Resolver is called', () => {
      const result = TestBed.runInInjectionContext(() =>
        wizardResolver(activatedRouteSnapshotSpy, routerStateSnapshotSpy),
      );

      then('should return true and access to the route', () => {
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
});
