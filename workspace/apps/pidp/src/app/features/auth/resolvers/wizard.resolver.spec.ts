/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import {
  ActivatedRouteSnapshot,
  Router,
  RouterStateSnapshot,
} from '@angular/router';

import { Observable, of } from 'rxjs';

import { randNumber } from '@ngneat/falso';
import { MockProfileStatus } from '@test/mock-profile-status';
import { Spy, createSpyFromClass, provideAutoSpy } from 'jest-auto-spies';

import { PartyService } from '@app/core/party/party.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';
import { ProfileStatus } from '@app/features/portal/models/profile-status.model';
import { PortalResource } from '@app/features/portal/portal-resource.service';

import { wizardResolver } from './wizard.resolver';

describe('wizardResolver', () => {
  let activatedRouteSnapshotSpy: Spy<ActivatedRouteSnapshot>;
  let routerStateSnapshotSpy: Spy<RouterStateSnapshot>;
  let partyServiceSpy: Spy<PartyService>;
  let portalResource: Spy<PortalResource>;
  let router: Router;

  let mockProfileStatus: ProfileStatus;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        {
          provide: PartyService,
          useValue: createSpyFromClass(PartyService, {
            gettersToSpyOn: ['partyId'],
          }),
        },
        provideAutoSpy(PortalResource),
        provideAutoSpy(Router),
      ],
    });

    partyServiceSpy = TestBed.inject(PartyService) as Spy<PartyService>;
    portalResource = TestBed.inject(PortalResource) as Spy<PortalResource>;
    router = TestBed.inject(Router);
    mockProfileStatus = MockProfileStatus.get();
  });

  given('partyId does not exists, profileStatus is null', (done) => {
    const partyId = randNumber({ min: 1 });
    partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
    portalResource.getProfileStatus.mockReturnValueOnce(of(null));

    when('Resolver is called', () => {
      const result = TestBed.runInInjectionContext(() =>
        wizardResolver(activatedRouteSnapshotSpy, routerStateSnapshotSpy),
      );

      then('should return null', () => {
        if (result instanceof Observable) {
          result.subscribe((value) => {
            try {
              expect(value).toBeNull();
              done();
            } catch (error: any) {
              done(error);
            }
          });
        }
      });
    });
  });

  given('partyId exists, Demographics StatusCode is AVAILABLE', (done) => {
    const partyId = randNumber({ min: 1 });
    partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
    mockProfileStatus.status.demographics.statusCode = StatusCode.AVAILABLE;
    portalResource.getProfileStatus.mockReturnValueOnce(of(mockProfileStatus));

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

  given(
    'partyId exists, College Certification StatusCode is AVAILABLE',
    (done) => {
      const partyId = randNumber({ min: 1 });
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
      mockProfileStatus.status.collegeCertification.statusCode =
        StatusCode.AVAILABLE;
      portalResource.getProfileStatus.mockReturnValueOnce(
        of(mockProfileStatus),
      );

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
    },
  );

  given('partyId exists, College Certification isComplete is FALSE', (done) => {
    const partyId = randNumber({ min: 1 });
    partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
    mockProfileStatus.status.collegeCertification.isComplete = false;
    portalResource.getProfileStatus.mockReturnValueOnce(of(mockProfileStatus));

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

  given(
    'partyId exists, User Access Agreement StatusCode is AVAILABLE',
    (done) => {
      const partyId = randNumber({ min: 1 });
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
      mockProfileStatus.status.userAccessAgreement.statusCode =
        StatusCode.AVAILABLE;
      portalResource.getProfileStatus.mockReturnValueOnce(
        of(mockProfileStatus),
      );

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
    },
  );

  given(
    'partyId exists, user has completed the wizard with a college licence',
    (done) => {
      const partyId = randNumber({ min: 1 });
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
      mockProfileStatus.status.demographics.statusCode = StatusCode.COMPLETED;
      mockProfileStatus.status.collegeCertification.isComplete = true;
      mockProfileStatus.status.collegeCertification.hasCpn = true;
      mockProfileStatus.status.userAccessAgreement.statusCode =
        StatusCode.COMPLETED;
      portalResource.getProfileStatus.mockReturnValueOnce(
        of(mockProfileStatus),
      );

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
    },
  );

  given(
    'partyId exists, user has completed the wizard but selected no college licence',
    (done) => {
      const partyId = randNumber({ min: 1 });
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
      mockProfileStatus.status.demographics.statusCode = StatusCode.COMPLETED;
      mockProfileStatus.status.collegeCertification.isComplete = true;
      mockProfileStatus.status.collegeCertification.hasCpn = false;
      mockProfileStatus.status.userAccessAgreement.statusCode =
        StatusCode.COMPLETED;
      portalResource.getProfileStatus.mockReturnValueOnce(
        of(mockProfileStatus),
      );

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
    },
  );
});
