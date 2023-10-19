/* eslint-disable @typescript-eslint/no-explicit-any */
import { HttpStatusCode } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';

import { randEmail, randFullName, randNumber } from '@ngneat/falso';
import { Spy, createSpyFromClass, provideAutoSpy } from 'jest-auto-spies';

import { PartyService } from '@app/core/party/party.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';
import { ProfileStatus } from '@app/features/portal/models/profile-status.model';

import { UserAccessAgreementResource } from './user-access-agreement-resource.service';
import { UserAccessAgreementResolver } from './user-access-agreement.resolver';

describe('UserAccessAgreementResolver', () => {
  let resolver: UserAccessAgreementResolver;
  let userAccessAgreementResourceSpy: Spy<UserAccessAgreementResource>;
  let partyServiceSpy: Spy<PartyService>;

  let mockProfileStatus: ProfileStatus;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        UserAccessAgreementResolver,
        provideAutoSpy(UserAccessAgreementResource),
        {
          provide: PartyService,
          useValue: createSpyFromClass(PartyService, {
            gettersToSpyOn: ['partyId'],
            settersToSpyOn: ['partyId'],
          }),
        },
      ],
    });

    resolver = TestBed.inject(UserAccessAgreementResolver);
    userAccessAgreementResourceSpy = TestBed.inject<any>(
      UserAccessAgreementResource
    );
    partyServiceSpy = TestBed.inject<any>(PartyService);

    mockProfileStatus = {
      alerts: [],
      status: {
        dashboardInfo: {
          displayFullName: randFullName(),
          collegeCode: randNumber(),
          statusCode: StatusCode.AVAILABLE,
        },
        demographics: {
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

      when('resolving the user access agreement status is successful', () => {
        userAccessAgreementResourceSpy.getProfileStatus
          .mustBeCalledWith(partyId)
          .nextOneTimeWith(mockProfileStatus);
        let actualResult: StatusCode | null;
        resolver
          .resolve()
          .subscribe(
            (profileStatus: StatusCode | null) => (actualResult = profileStatus)
          );

        then(
          'response will provide the status code for user access agreement',
          () => {
            expect(
              userAccessAgreementResourceSpy.getProfileStatus
            ).toHaveBeenCalledTimes(1);
            expect(actualResult).toBe(
              mockProfileStatus.status.userAccessAgreement.statusCode
            );
          }
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
        resolver
          .resolve()
          .subscribe(
            (profileStatus: StatusCode | null) => (actualResult = profileStatus)
          );

        then(
          'response will provide null as status code for user access agreement',
          () => {
            expect(
              userAccessAgreementResourceSpy.getProfileStatus
            ).toHaveBeenCalledTimes(1);
            expect(actualResult).toBe(null);
          }
        );
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

        then(
          'response will provide null as status code for user access agreement',
          () => {
            expect(
              userAccessAgreementResourceSpy.getProfileStatus
            ).not.toHaveBeenCalled();
            expect(actualResult).toBe(null);
          }
        );
      });
    });
  });
});
