import { TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';

import { MockProfileStatus } from '@test/mock-profile-status';
import { provideAutoSpy } from 'jest-auto-spies';

import { StatusCode } from '../../enums/status-code.enum';
import { ProfileStatus } from '../../models/profile-status.model';
import { FacilityDetailsPortalSection } from './facility-details-portal-section.class';

describe('FacilityDetailsPortalSection', () => {
  let router: Router;
  let mockProfileStatus: ProfileStatus;
  let section: FacilityDetailsPortalSection;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideAutoSpy(Router)],
    });

    router = TestBed.inject(Router);
    mockProfileStatus = MockProfileStatus.get();

    section = new FacilityDetailsPortalSection(mockProfileStatus, router);
  });

  describe('INIT', () => {
    given('default status and ProfileStatus', () => {
      when('the class is instanciated', () => {
        then('property are filled out', () => {
          expect(section.key).toBe('facilityDetails');
          expect(section.heading).toBe('Facility Details');
          expect(section.description).toBe(
            `Provide details about your work place (facility).`,
          );
        });
      });
    });
  });

  describe('PROPERTY: get hint', () => {
    given('facilityDetails StatusCode is AVAILABLE', () => {
      mockProfileStatus.status.facilityDetails.statusCode =
        StatusCode.AVAILABLE;
      when('the class is instanciated', () => {
        then('hint should return a text', () => {
          expect(section.hint).toBe('2 minutes to complete');
        });
      });
    });

    given('facilityDetails StatusCode is COMPLETED', () => {
      mockProfileStatus.status.facilityDetails.statusCode =
        StatusCode.COMPLETED;
      when('the class is instanciated', () => {
        then('hint should return an empty text', () => {
          expect(section.hint).toBe('');
        });
      });
    });
  });

  describe('PROPERTY: get action', () => {
    given('demographics StatusCode is AVAILABLE', () => {
      mockProfileStatus.status.demographics.statusCode = StatusCode.AVAILABLE;
      when('the class is instanciated', () => {
        then(
          'action should return a PortalSectionAction object with disabled property set to true',
          () => {
            expect(section.action).toStrictEqual({
              label: 'Update',
              route: '/organization-info/facility-details',
              disabled: true,
            });
          },
        );
      });
    });

    given('demographics StatusCode is COMPLETED', () => {
      mockProfileStatus.status.demographics.statusCode = StatusCode.COMPLETED;
      when('the class is instanciated', () => {
        then(
          'action should return a PortalSectionAction object with disabled property set to false',
          () => {
            expect(section.action).toStrictEqual({
              label: 'Update',
              route: '/organization-info/facility-details',
              disabled: false,
            });
          },
        );
      });
    });
  });

  describe('PROPERTY: get statusType', () => {
    given('facilityDetails StatusCode is AVAILABLE', () => {
      mockProfileStatus.status.facilityDetails.statusCode =
        StatusCode.AVAILABLE;
      when('the class is instanciated', () => {
        then('should return a string "warn"', () => {
          expect(section.statusType).toBe('warn');
        });
      });
    });

    given('facilityDetails StatusCode is COMPLETED', () => {
      mockProfileStatus.status.facilityDetails.statusCode =
        StatusCode.COMPLETED;
      when('the class is instanciated', () => {
        then('should return a string "success"', () => {
          expect(section.statusType).toBe('success');
        });
      });
    });

    given('facilityDetails StatusCode is ERROR', () => {
      mockProfileStatus.status.facilityDetails.statusCode = StatusCode.ERROR;
      when('the class is instanciated', () => {
        then('should return a string "danger"', () => {
          expect(section.statusType).toBe('danger');
        });
      });
    });
  });

  describe('PROPERTY: get status', () => {
    given('facilityDetails StatusCode is AVAILABLE', () => {
      mockProfileStatus.status.facilityDetails.statusCode =
        StatusCode.AVAILABLE;
      when('the class is instanciated', () => {
        then('should return a string "Incomplete"', () => {
          expect(section.status).toBe('Incomplete');
        });
      });
    });

    given('facilityDetails StatusCode is COMPLETED', () => {
      mockProfileStatus.status.facilityDetails.statusCode =
        StatusCode.COMPLETED;
      when('the class is instanciated', () => {
        then('should return a string "Completed"', () => {
          expect(section.status).toBe('Completed');
        });
      });
    });
  });

  describe('METHOD: performAction', () => {
    given('the class is instanciated', () => {
      when('performAction function is called', () => {
        section.performAction();
        then('the user navigate to facility-details page', () => {
          expect(router.navigate).toHaveBeenCalledWith([
            '///organization-info/facility-details',
          ]);
        });
      });
    });
  });
});
