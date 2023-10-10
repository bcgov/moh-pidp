/* eslint-disable @typescript-eslint/no-explicit-any */
import { HttpStatusCode } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';

import {
  randEmail,
  randFirstName,
  randFullName,
  randLastName,
  randNumber,
  randPhoneNumber,
} from '@ngneat/falso';
import { Spy, createSpyFromClass, provideAutoSpy } from 'jest-auto-spies';

import { PartyService } from '@app/core/party/party.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';
import { ProfileStatus } from '@app/features/portal/models/profile-status.model';

import { DriverFitnessResource } from './driver-fitness-resource.service';
import { DriverFitnessResolver } from './driver-fitness.resolver';

describe('DriverFitnessResolver', () => {
  let resolver: DriverFitnessResolver;
  let driverFitnessResourceSpy: Spy<DriverFitnessResource>;
  let partyServiceSpy: Spy<PartyService>;

  let mockProfileStatus: ProfileStatus;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        DriverFitnessResolver,
        provideAutoSpy(DriverFitnessResource),
        {
          provide: PartyService,
          useValue: createSpyFromClass(PartyService, {
            gettersToSpyOn: ['partyId'],
            settersToSpyOn: ['partyId'],
          }),
        },
      ],
    });

    resolver = TestBed.inject(DriverFitnessResolver);
    driverFitnessResourceSpy = TestBed.inject<any>(DriverFitnessResource);
    partyServiceSpy = TestBed.inject<any>(PartyService);

    mockProfileStatus = {
      alerts: [],
      status: {
        dashboardInfo: {
          fullName: randFullName(),
          collegeCode: randNumber(),
          statusCode: StatusCode.AVAILABLE,
        },
        demographics: {
          firstName: randFirstName(),
          lastName: randLastName(),
          email: randEmail(),
          phone: randPhoneNumber(),
          statusCode: StatusCode.AVAILABLE,
        },
        collegeCertification: {
          hasCpn: false,
          licenceDeclared: false,
          statusCode: StatusCode.AVAILABLE,
          isComplete: false,
        },
        administratorInfo: {
          email: randEmail(),
          statusCode: StatusCode.AVAILABLE,
        },
        organizationDetails: { statusCode: StatusCode.AVAILABLE },
        facilityDetails: { statusCode: StatusCode.AVAILABLE },
        endorsements: { statusCode: StatusCode.AVAILABLE },
        userAccessAgreement: { statusCode: StatusCode.AVAILABLE },
        saEforms: {
          statusCode: StatusCode.AVAILABLE,
          incorrectLicenceType: false,
        },
        prescriptionRefillEforms: { statusCode: StatusCode.AVAILABLE },
        'prescription-refill-eforms': { statusCode: StatusCode.AVAILABLE },
        bcProvider: { statusCode: StatusCode.AVAILABLE },
        hcimAccountTransfer: { statusCode: StatusCode.AVAILABLE },
        hcimEnrolment: { statusCode: StatusCode.AVAILABLE },
        driverFitness: { statusCode: StatusCode.AVAILABLE },
        msTeamsPrivacyOfficer: { statusCode: StatusCode.AVAILABLE },
        msTeamsClinicMember: { statusCode: StatusCode.AVAILABLE },
        providerReportingPortal: { statusCode: StatusCode.AVAILABLE },
        'provider-reporting-portal': { statusCode: StatusCode.AVAILABLE },
        sitePrivacySecurityChecklist: { statusCode: StatusCode.AVAILABLE },
        complianceTraining: { statusCode: StatusCode.AVAILABLE },
        primaryCareRostering: { statusCode: StatusCode.AVAILABLE },
        immsBCEforms: { statusCode: StatusCode.AVAILABLE },
      },
    };
  });

  describe('METHOD: resolve', () => {
    given('partyId exists', () => {
      const partyId = randNumber({ min: 1 });
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);

      when('resolving the SA eForms enrolment status is successful', () => {
        driverFitnessResourceSpy.getProfileStatus
          .mustBeCalledWith(partyId)
          .nextOneTimeWith(mockProfileStatus);
        let actualResult: StatusCode | null;
        resolver
          .resolve()
          .subscribe(
            (profileStatus: StatusCode | null) => (actualResult = profileStatus)
          );

        then('response will provide the status code for SA eForms', () => {
          expect(
            driverFitnessResourceSpy.getProfileStatus
          ).toHaveBeenCalledTimes(1);
          expect(actualResult).toBe(
            mockProfileStatus.status.driverFitness.statusCode
          );
        });
      });
    });

    given('partyId exists', () => {
      const partyId = randNumber({ min: 1 });
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
      when('resolving the SA eForms enrolment status is unsuccessful', () => {
        driverFitnessResourceSpy.getProfileStatus
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
          expect(
            driverFitnessResourceSpy.getProfileStatus
          ).toHaveBeenCalledTimes(1);
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
          expect(driverFitnessResourceSpy.requestAccess).not.toHaveBeenCalled();
          expect(actualResult).toBe(null);
        });
      });
    });
  });
});
