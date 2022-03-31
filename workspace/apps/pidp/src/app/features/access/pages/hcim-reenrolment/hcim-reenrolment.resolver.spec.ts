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
import { ProfileStatus } from '@app/features/portal/sections/models/profile-status.model';

import { HcimReenrolmentResource } from './hcim-reenrolment-resource.service';
import { HcimReenrolmentResolver } from './hcim-reenrolment.resolver';

describe('HcimReenrolmentResolver', () => {
  let resolver: HcimReenrolmentResolver;
  let hcimReenrolmentResourceSpy: Spy<HcimReenrolmentResource>;
  let partyServiceSpy: Spy<PartyService>;

  let mockProfileStatus: ProfileStatus;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        HcimReenrolmentResolver,
        provideAutoSpy(HcimReenrolmentResource),
        {
          provide: PartyService,
          useValue: createSpyFromClass(PartyService, {
            gettersToSpyOn: ['partyId'],
            settersToSpyOn: ['partyId'],
          }),
        },
      ],
    });

    resolver = TestBed.inject(HcimReenrolmentResolver);
    hcimReenrolmentResourceSpy = TestBed.inject<any>(HcimReenrolmentResource);
    partyServiceSpy = TestBed.inject<any>(PartyService);

    mockProfileStatus = {
      alerts: [],
      status: {
        demographics: {
          firstName: randFirstName(),
          lastName: randLastName(),
          email: randEmail(),
          phone: randPhoneNumber(),
          statusCode: 1,
        },
        collegeCertification: {
          collegeCode: randNumber(),
          licenceNumber: randText(),
          statusCode: 3,
        },
        userAccessAgreement: { statusCode: 1 },
        saEforms: { statusCode: 3 },
        hcim: { statusCode: 1 },
        sitePrivacySecurityChecklist: { statusCode: 1 },
        complianceTraining: { statusCode: 1 },
        transactions: { statusCode: 1 },
      },
    };
  });

  describe('METHOD: resolve', () => {
    given('partyId exists', () => {
      const partyId = randNumber({ min: 1 });
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);

      when(
        'resolving the HCIM web account transfer status is successful',
        () => {
          hcimReenrolmentResourceSpy.getProfileStatus
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
            'response will provide the status code for HCIM web account transfer',
            () => {
              expect(
                hcimReenrolmentResourceSpy.getProfileStatus
              ).toHaveBeenCalledTimes(1);
              expect(actualResult).toBe(
                mockProfileStatus.status.hcim.statusCode
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
        'resolving the HCIM web account transfer status is unsuccessful',
        () => {
          hcimReenrolmentResourceSpy.getProfileStatus
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
            'response will provide null as status code for HCIM web account transfer',
            () => {
              expect(
                hcimReenrolmentResourceSpy.getProfileStatus
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
          'response will provide null as status code for HCIM web account transfer',
          () => {
            expect(
              hcimReenrolmentResourceSpy.requestAccess
            ).not.toHaveBeenCalled();
            expect(actualResult).toBe(null);
          }
        );
      });
    });
  });
});
