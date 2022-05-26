/* eslint-disable @typescript-eslint/no-explicit-any */
import { HttpStatusCode } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';

import {
  randEmail,
  randFirstName,
  randLastName,
  randNumber,
  randPhoneNumber,
  randText,
} from '@ngneat/falso';
import { Spy, createSpyFromClass, provideAutoSpy } from 'jest-auto-spies';

import { PartyService } from '@app/core/party/party.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';
import { ProfileStatus } from '@app/features/portal/models/profile-status.model';

import { HcimAccountTransferResource } from './hcim-account-transfer-resource.service';
import { HcimAccountTransferResolver } from './hcim-account-transfer.resolver';

describe('HcimAccountTransferResolver', () => {
  let resolver: HcimAccountTransferResolver;
  let hcimAccountTransferResourceSpy: Spy<HcimAccountTransferResource>;
  let partyServiceSpy: Spy<PartyService>;

  let mockProfileStatus: ProfileStatus;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        HcimAccountTransferResolver,
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

    resolver = TestBed.inject(HcimAccountTransferResolver);
    hcimAccountTransferResourceSpy = TestBed.inject<any>(
      HcimAccountTransferResource
    );
    partyServiceSpy = TestBed.inject<any>(PartyService);

    mockProfileStatus = {
      alerts: [],
      status: {
        demographics: {
          firstName: randFirstName(),
          lastName: randLastName(),
          email: randEmail(),
          phone: randPhoneNumber(),
          statusCode: StatusCode.AVAILABLE,
        },
        collegeCertification: {
          collegeCode: randNumber(),
          licenceNumber: randText(),
          statusCode: StatusCode.AVAILABLE,
        },
        administratorInfo: {
          email: randEmail(),
          statusCode: StatusCode.AVAILABLE,
        },
        organizationDetails: { statusCode: StatusCode.AVAILABLE },
        facilityDetails: { statusCode: StatusCode.AVAILABLE },
        userAccessAgreement: { statusCode: StatusCode.AVAILABLE },
        saEforms: { statusCode: StatusCode.AVAILABLE },
        hcimAccountTransfer: { statusCode: StatusCode.AVAILABLE },
        hcimEnrolment: { statusCode: StatusCode.AVAILABLE },
        sitePrivacySecurityChecklist: { statusCode: StatusCode.AVAILABLE },
        complianceTraining: { statusCode: StatusCode.AVAILABLE },
      },
    };
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
          resolver
            .resolve()
            .subscribe(
              (profileStatus: StatusCode | null) =>
                (actualResult = profileStatus)
            );

          then(
            'response will provide the status code for HCIMWeb Account Transfer',
            () => {
              expect(
                hcimAccountTransferResourceSpy.getProfileStatus
              ).toHaveBeenCalledTimes(1);
              expect(actualResult).toBe(
                mockProfileStatus.status.hcimAccountTransfer.statusCode
              );
            }
          );
        }
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
          resolver
            .resolve()
            .subscribe(
              (profileStatus: StatusCode | null) =>
                (actualResult = profileStatus)
            );

          then(
            'response will provide null as status code for HCIMWeb Account Transfer',
            () => {
              expect(
                hcimAccountTransferResourceSpy.getProfileStatus
              ).toHaveBeenCalledTimes(1);
              expect(actualResult).toBe(null);
            }
          );
        }
      );
    });

    given('partyId does not exists', () => {
      const partyId = null;
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);

      when('attempting to resolve the status code', () => {
        let actualResult: StatusCode | null;
        resolver
          .resolve()
          .subscribe(
            (profileStatus: StatusCode | null) => (actualResult = profileStatus)
          );

        then(
          'response will provide null as status code for HCIMWeb Account Transfer',
          () => {
            expect(
              hcimAccountTransferResourceSpy.requestAccess
            ).not.toHaveBeenCalled();
            expect(actualResult).toBe(null);
          }
        );
      });
    });
  });
});
