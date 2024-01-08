import { TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';

import { MockProfileStatus } from '@test/mock-profile-status';
import { provideAutoSpy } from 'jest-auto-spies';

import { StatusCode } from '../../enums/status-code.enum';
import { ProfileStatus } from '../../models/profile-status.model';
import { OrganizationDetailsPortalSection } from './organization-details-portal-section.class';

describe('OrganizationDetailsPortalSection', () => {
  let router: Router;
  let mockProfileStatus: ProfileStatus;
  let section: OrganizationDetailsPortalSection;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideAutoSpy(Router)],
    });

    router = TestBed.inject(Router);
    mockProfileStatus = MockProfileStatus.get();

    section = new OrganizationDetailsPortalSection(mockProfileStatus, router);
  });

  describe('INIT', () => {
    given('default status and ProfileStatus', () => {
      when('the class is instanciated', () => {
        then('property are filled out', () => {
          expect(section.key).toBe('organizationDetails');
          expect(section.heading).toBe('Organization Details');
          expect(section.description).toBe(
            `Provide details about your organization.`,
          );
        });
      });
    });
  });

  describe('PROPERTY: get hint', () => {
    given('organizationDetails StatusCode is AVAILABLE', () => {
      mockProfileStatus.status.organizationDetails.statusCode =
        StatusCode.AVAILABLE;
      when('the class is instanciated', () => {
        then('hint should return a text', () => {
          expect(section.hint).toBe('2 minutes to complete');
        });
      });
    });

    given('organizationDetails StatusCode is COMPLETED', () => {
      mockProfileStatus.status.organizationDetails.statusCode =
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
              route: '/organization-info/organization-details',
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
              route: '/organization-info/organization-details',
              disabled: false,
            });
          },
        );
      });
    });
  });

  describe('PROPERTY: get statusType', () => {
    given('organizationDetails StatusCode is AVAILABLE', () => {
      mockProfileStatus.status.organizationDetails.statusCode =
        StatusCode.AVAILABLE;
      when('the class is instanciated', () => {
        then('should return a string "warn"', () => {
          expect(section.statusType).toBe('warn');
        });
      });
    });

    given('organizationDetails StatusCode is COMPLETED', () => {
      mockProfileStatus.status.organizationDetails.statusCode =
        StatusCode.COMPLETED;
      when('the class is instanciated', () => {
        then('should return a string "success"', () => {
          expect(section.statusType).toBe('success');
        });
      });
    });

    given('organizationDetails StatusCode is ERROR', () => {
      mockProfileStatus.status.organizationDetails.statusCode =
        StatusCode.ERROR;
      when('the class is instanciated', () => {
        then('should return a string "danger"', () => {
          expect(section.statusType).toBe('danger');
        });
      });
    });
  });

  describe('PROPERTY: get status', () => {
    given('organizationDetails StatusCode is AVAILABLE', () => {
      mockProfileStatus.status.organizationDetails.statusCode =
        StatusCode.AVAILABLE;
      when('the class is instanciated', () => {
        then('should return a string "Incomplete"', () => {
          expect(section.status).toBe('Incomplete');
        });
      });
    });

    given('organizationDetails StatusCode is COMPLETED', () => {
      mockProfileStatus.status.organizationDetails.statusCode =
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
        then('the user navigate to organization-details page', () => {
          expect(router.navigate).toHaveBeenCalledWith([
            '///organization-info/organization-details',
          ]);
        });
      });
    });
  });
});
