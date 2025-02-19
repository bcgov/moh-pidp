/* eslint-disable @typescript-eslint/no-explicit-any */
import { HttpStatusCode } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import {
  ActivatedRouteSnapshot,
  ResolveFn,
  RouterStateSnapshot,
} from '@angular/router';

import { Observable } from 'rxjs';

import { randNumber } from '@ngneat/falso';
import { MockProfileStatus } from '@test/mock-profile-status';
import { Spy, createSpyFromClass, provideAutoSpy } from 'jest-auto-spies';

import { PartyService } from '@app/core/party/party.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';
import { ProfileStatus } from '@app/features/portal/models/profile-status.model';

import { UserAccessAgreementResource } from './user-access-agreement-resource.service';
import { userAccessAgreementResolver } from './user-access-agreement.resolver';

describe('userAccessAgreementResolver', () => {
  const executeResolver: ResolveFn<StatusCode | null> = (
    ...resolverParameters
  ) =>
    TestBed.runInInjectionContext(() =>
      userAccessAgreementResolver(...resolverParameters),
    );

  let activatedRouteSnapshotSpy: Spy<ActivatedRouteSnapshot>;
  let routerStateSnapshotSpy: Spy<RouterStateSnapshot>;
  let userAccessAgreementResourceSpy: Spy<UserAccessAgreementResource>;
  let partyServiceSpy: Spy<PartyService>;

  let mockProfileStatus: ProfileStatus;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideAutoSpy(UserAccessAgreementResource),
        provideAutoSpy(ActivatedRouteSnapshot),
        provideAutoSpy(RouterStateSnapshot),
        {
          provide: PartyService,
          useValue: createSpyFromClass(PartyService, {
            gettersToSpyOn: ['partyId'],
            settersToSpyOn: ['partyId'],
          }),
        },
      ],
    });

    userAccessAgreementResourceSpy = TestBed.inject<any>(
      UserAccessAgreementResource,
    );
    partyServiceSpy = TestBed.inject<any>(PartyService);
    activatedRouteSnapshotSpy = TestBed.inject<any>(ActivatedRouteSnapshot);
    routerStateSnapshotSpy = TestBed.inject<any>(RouterStateSnapshot);
    mockProfileStatus = MockProfileStatus.get();
  });

  describe('METHOD: resolve', () => {
    given('partyId exists', () => {
      const partyId = randNumber({ min: 1 });
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);

      when('resolving the user access agreement status is successful', () => {
        userAccessAgreementResourceSpy.getProfileStatus
          .mustBeCalledWith(partyId)
          .nextOneTimeWith(mockProfileStatus);
        let actualResult: StatusCode | null;
        (
          executeResolver(
            activatedRouteSnapshotSpy,
            routerStateSnapshotSpy,
          ) as Observable<StatusCode | null>
        ).subscribe(
          (profileStatus: StatusCode | null) => (actualResult = profileStatus),
        );

        then(
          'response will provide the status code for user access agreement',
          () => {
            expect(
              userAccessAgreementResourceSpy.getProfileStatus,
            ).toHaveBeenCalledTimes(1);
            expect(actualResult).toBe(
              mockProfileStatus.status.userAccessAgreement.statusCode,
            );
          },
        );
      });
    });

    given('partyId exists', () => {
      const partyId = randNumber({ min: 1 });
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
      when('resolving the user access agreement status is unsuccessful', () => {
        userAccessAgreementResourceSpy.getProfileStatus
          .mustBeCalledWith(partyId)
          .nextWithValues([
            {
              errorValue: {
                status: HttpStatusCode.NotFound,
              },
            },
          ]);
        let actualResult: StatusCode | null;
        (
          executeResolver(
            activatedRouteSnapshotSpy,
            routerStateSnapshotSpy,
          ) as Observable<StatusCode | null>
        ).subscribe(
          (profileStatus: StatusCode | null) => (actualResult = profileStatus),
        );

        then(
          'response will provide null as status code for user access agreement',
          () => {
            expect(
              userAccessAgreementResourceSpy.getProfileStatus,
            ).toHaveBeenCalledTimes(1);
            expect(actualResult).toBe(null);
          },
        );
      });
    });

    given('partyId does not exists', () => {
      const partyId = null;
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);

      when('attempting to resolve the status code', () => {
        let actualResult: StatusCode | null;
        (
          executeResolver(
            activatedRouteSnapshotSpy,
            routerStateSnapshotSpy,
          ) as Observable<StatusCode | null>
        ).subscribe(
          (profileStatus: StatusCode | null) => (actualResult = profileStatus),
        );

        then(
          'response will provide null as status code for user access agreement',
          () => {
            expect(
              userAccessAgreementResourceSpy.getProfileStatus,
            ).not.toHaveBeenCalled();
            expect(actualResult).toBe(null);
          },
        );
      });
    });
  });
});
