import { TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';

import { MockProfileStatus } from '@test/mock-profile-status';
import { provideAutoSpy } from 'jest-auto-spies';

import { StatusCode } from '../../enums/status-code.enum';
import { ProfileStatus } from '../../models/profile-status.model';
import { AdministratorInfoPortalSection } from './administrator-information-portal-section';

describe('AdministratorInfoPortalSection', () => {
  let router: Router;
  let mockProfileStatus: ProfileStatus;
  let section: AdministratorInfoPortalSection;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideAutoSpy(Router)],
    });

    router = TestBed.inject(Router);
    mockProfileStatus = MockProfileStatus.get();

    section = new AdministratorInfoPortalSection(mockProfileStatus, router);
  });

  describe('INIT', () => {
    given('default status and ProfileStatus', () => {
      when('the class is instanciated', () => {
        then('property are filled out', () => {
          expect(section.key).toBe('facilityDetails');
          expect(section.heading).toBe('Administrator Information');
          expect(section.description).toBe(
            `Provide your office administrator's contact information.`,
          );
        });
      });
    });
  });

  describe('PROPERTY: get hint', () => {
    given('administratorInfo StatusCode is AVAILABLE', () => {
      mockProfileStatus.status.administratorInfo.statusCode =
        StatusCode.AVAILABLE;
      when('the class is instanciated', () => {
        then('hint should return a text', () => {
          expect(section.hint).toBe('1 minute to complete');
        });
      });
    });

    given('administratorInfo StatusCode is COMPLETED', () => {
      mockProfileStatus.status.administratorInfo.statusCode =
        StatusCode.COMPLETED;
      when('the class is instanciated', () => {
        then('hint should return an empty text', () => {
          expect(section.hint).toBe('');
        });
      });
    });
  });

  describe('PROPERTY: get properties', () => {
    given('administratorInfo StatusCode is AVAILABLE', () => {
      mockProfileStatus.status.administratorInfo.statusCode =
        StatusCode.AVAILABLE;
      when('the class is instanciated', () => {
        then('properties should return an empty array', () => {
          expect(section.properties).toStrictEqual([]);
        });
      });
    });

    given('administratorInfo StatusCode is COMPLETED', () => {
      mockProfileStatus.status.administratorInfo.statusCode =
        StatusCode.COMPLETED;
      when('the class is instanciated', () => {
        then('properties should return an array with 1 property', () => {
          expect(section.properties).toStrictEqual([
            {
              key: 'email',
              value: mockProfileStatus.status.administratorInfo.email,
              label: 'Access Administrator Email:',
            },
          ]);
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
              route: '/organization-info/administrator-info',
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
              route: '/organization-info/administrator-info',
              disabled: false,
            });
          },
        );
      });
    });
  });

  describe('PROPERTY: get statusType', () => {
    given('administratorInfo StatusCode is AVAILABLE', () => {
      mockProfileStatus.status.administratorInfo.statusCode =
        StatusCode.AVAILABLE;
      when('the class is instanciated', () => {
        then('should return a string "warn"', () => {
          expect(section.statusType).toBe('warn');
        });
      });
    });

    given('administratorInfo StatusCode is COMPLETED', () => {
      mockProfileStatus.status.administratorInfo.statusCode =
        StatusCode.COMPLETED;
      when('the class is instanciated', () => {
        then('should return a string "success"', () => {
          expect(section.statusType).toBe('success');
        });
      });
    });

    given('administratorInfo StatusCode is ERROR', () => {
      mockProfileStatus.status.administratorInfo.statusCode = StatusCode.ERROR;
      when('the class is instanciated', () => {
        then('should return a string "danger"', () => {
          expect(section.statusType).toBe('danger');
        });
      });
    });
  });

  describe('PROPERTY: get status', () => {
    given('administratorInfo StatusCode is AVAILABLE', () => {
      mockProfileStatus.status.administratorInfo.statusCode =
        StatusCode.AVAILABLE;
      when('the class is instanciated', () => {
        then('should return a string "Incomplete"', () => {
          expect(section.status).toBe('Incomplete');
        });
      });
    });

    given('administratorInfo StatusCode is COMPLETED', () => {
      mockProfileStatus.status.administratorInfo.statusCode =
        StatusCode.COMPLETED;
      when('the class is instanciated', () => {
        then('should return a string "Completed"', () => {
          expect(section.status).toBe('Completed');
        });
      });
    });
  });
});
