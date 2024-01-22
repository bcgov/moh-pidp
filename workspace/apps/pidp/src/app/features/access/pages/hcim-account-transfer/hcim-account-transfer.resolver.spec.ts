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

import { HcimAccountTransferResource } from './hcim-account-transfer-resource.service';
import { hcimAccountTransferResolver } from './hcim-account-transfer.resolver';

describe('hcimAccountTransferResolver', () => {
  const executeResolver: ResolveFn<StatusCode | null> = (
    ...resolverParameters
  ) =>
    TestBed.runInInjectionContext(() =>
      hcimAccountTransferResolver(...resolverParameters),
    );

  let hcimAccountTransferResourceSpy: Spy<HcimAccountTransferResource>;
  let partyServiceSpy: Spy<PartyService>;
  let activatedRouteSnapshotSpy: Spy<ActivatedRouteSnapshot>;
  let routerStateSnapshotSpy: Spy<RouterStateSnapshot>;

  let mockProfileStatus: ProfileStatus;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideAutoSpy(HcimAccountTransferResource),
        {
          provide: PartyService,
          useValue: createSpyFromClass(PartyService, {
            gettersToSpyOn: ['partyId'],
            settersToSpyOn: ['partyId'],
          }),
        },
      ],
    });

    hcimAccountTransferResourceSpy = TestBed.inject<any>(
      HcimAccountTransferResource,
    );
    partyServiceSpy = TestBed.inject<any>(PartyService);

    mockProfileStatus = MockProfileStatus.get();
  });

  describe('METHOD: resolve', () => {
    given('partyId exists', () => {
      const partyId = randNumber({ min: 1 });
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);

      when(
        'resolving the HCIMWeb Account Transfer status is successful',
        () => {
          hcimAccountTransferResourceSpy.getProfileStatus
            .mustBeCalledWith(partyId)
            .nextOneTimeWith(mockProfileStatus);
          let actualResult: StatusCode | null;
          (
            executeResolver(
              activatedRouteSnapshotSpy,
              routerStateSnapshotSpy,
            ) as Observable<StatusCode | null>
          ).subscribe(
            (profileStatus: StatusCode | null) =>
              (actualResult = profileStatus),
          );

          then(
            'response will provide the status code for HCIMWeb Account Transfer',
            () => {
              expect(
                hcimAccountTransferResourceSpy.getProfileStatus,
              ).toHaveBeenCalledTimes(1);
              expect(actualResult).toBe(
                mockProfileStatus.status.hcimAccountTransfer.statusCode,
              );
            },
          );
        },
      );
    });

    given('partyId exists', () => {
      const partyId = randNumber({ min: 1 });
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
      when(
        'resolving the HCIMWeb Account Transfer status is unsuccessful',
        () => {
          hcimAccountTransferResourceSpy.getProfileStatus
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
            (profileStatus: StatusCode | null) =>
              (actualResult = profileStatus),
          );

          then(
            'response will provide null as status code for HCIMWeb Account Transfer',
            () => {
              expect(
                hcimAccountTransferResourceSpy.getProfileStatus,
              ).toHaveBeenCalledTimes(1);
              expect(actualResult).toBe(null);
            },
          );
        },
      );
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
          'response will provide null as status code for HCIMWeb Account Transfer',
          () => {
            expect(
              hcimAccountTransferResourceSpy.requestAccess,
            ).not.toHaveBeenCalled();
            expect(actualResult).toBe(null);
          },
        );
      });
    });
  });
});
