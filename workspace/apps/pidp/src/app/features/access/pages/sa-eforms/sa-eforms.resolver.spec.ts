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

import { SaEformsResource } from './sa-eforms-resource.service';
import { SaEformsResolver } from './sa-eforms.resolver';

describe('SaEformsResolver', () => {
  let resolver: SaEformsResolver;
  let saEformsResourceSpy: Spy<SaEformsResource>;
  let partyServiceSpy: Spy<PartyService>;

  let mockProfileStatus: ProfileStatus;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        SaEformsResolver,
        provideAutoSpy(SaEformsResource),
        {
          provide: PartyService,
          useValue: createSpyFromClass(PartyService, {
            gettersToSpyOn: ['partyId'],
            settersToSpyOn: ['partyId'],
          }),
        },
      ],
    });

    resolver = TestBed.inject(SaEformsResolver);
    saEformsResourceSpy = TestBed.inject<any>(SaEformsResource);
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

      when('resolving the SA eForms enrolment status is successful', () => {
        saEformsResourceSpy.getProfileStatus
          .mustBeCalledWith(partyId)
          .nextOneTimeWith(mockProfileStatus);
        let actualResult: StatusCode | null;
        resolver
          .resolve()
          .subscribe(
            (profileStatus: StatusCode | null) => (actualResult = profileStatus)
          );

        then('response will provide the status code for SA eForms', () => {
          expect(saEformsResourceSpy.getProfileStatus).toHaveBeenCalledTimes(1);
          expect(actualResult).toBe(
            mockProfileStatus.status.saEforms.statusCode
          );
        });
      });
    });

    given('partyId exists', () => {
      const partyId = randNumber({ min: 1 });
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
      when('resolving the SA eForms enrolment status is unsuccessful', () => {
        saEformsResourceSpy.getProfileStatus
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
            (profileStatus: StatusCode | null) => (actualResult = profileStatus)
          );

        then('response will provide null as status code for SA eForms', () => {
          expect(saEformsResourceSpy.getProfileStatus).toHaveBeenCalledTimes(1);
          expect(actualResult).toBe(null);
        });
      });
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

        then('response will provide null as status code for SA eForms', () => {
          expect(saEformsResourceSpy.requestAccess).not.toHaveBeenCalled();
          expect(actualResult).toBe(null);
        });
      });
    });
  });
});
