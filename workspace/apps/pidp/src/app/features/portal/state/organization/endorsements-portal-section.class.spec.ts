import { TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';

import { MockProfileStatus } from '@test/mock-profile-status';
import { provideAutoSpy } from 'jest-auto-spies';

import { StatusCode } from '../../enums/status-code.enum';
import { ProfileStatus } from '../../models/profile-status.model';
import { EndorsementsPortalSection } from './endorsements-portal-section.class';

describe('EndorsementsPortalSection', () => {
  let router: Router;
  let mockProfileStatus: ProfileStatus;
  let section: EndorsementsPortalSection;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideAutoSpy(Router)],
    });

    router = TestBed.inject(Router);
    mockProfileStatus = MockProfileStatus.get();

    section = new EndorsementsPortalSection(mockProfileStatus, router);
  });

  describe('INIT', () => {
    given('default status and ProfileStatus', () => {
      when('the class is instanciated', () => {
        then('property are filled out', () => {
          expect(section.key).toBe('endorsements');
          expect(section.heading).toBe('Endorsements');
          expect(section.description).toBe(
            `View and make changes to your care team. Endorse or request endorsements from colleagues on your health care team to access various systems.`,
          );
        });
      });
    });
  });

  describe('PROPERTY: get hint', () => {
    given('the class is instanciated', () => {
      when('get hint() is called', () => {
        then('hint should return a text', () => {
          expect(section.hint).toBe('1 minute to complete');
        });
      });
    });
  });

  describe('PROPERTY: get action', () => {
    given('endorsements StatusCode is AVAILABLE', () => {
      mockProfileStatus.status.endorsements.statusCode = StatusCode.AVAILABLE;
      when('the class is instanciated', () => {
        then(
          'action should return a PortalSectionAction object with disabled property set to false',
          () => {
            expect(section.action).toStrictEqual({
              label: 'View',
              route: '/organization-info/endorsements',
              disabled: false,
            });
          },
        );
      });
    });

    given('endorsements StatusCode is NOT_AVAILABLE', () => {
      mockProfileStatus.status.endorsements.statusCode =
        StatusCode.NOT_AVAILABLE;
      when('the class is instanciated', () => {
        then(
          'action should return a PortalSectionAction object with disabled property set to true',
          () => {
            expect(section.action).toStrictEqual({
              label: 'View',
              route: '/organization-info/endorsements',
              disabled: true,
            });
          },
        );
      });
    });
  });

  describe('PROPERTY: get statusType', () => {
    given('endorsements StatusCode is AVAILABLE', () => {
      mockProfileStatus.status.endorsements.statusCode = StatusCode.AVAILABLE;
      when('the class is instanciated', () => {
        then('should return a string "warn"', () => {
          expect(section.statusType).toBe('warn');
        });
      });
    });

    given('endorsements StatusCode is COMPLETED', () => {
      mockProfileStatus.status.endorsements.statusCode = StatusCode.COMPLETED;
      when('the class is instanciated', () => {
        then('should return a string "success"', () => {
          expect(section.statusType).toBe('success');
        });
      });
    });
  });

  describe('PROPERTY: get status', () => {
    given('endorsements StatusCode is AVAILABLE', () => {
      mockProfileStatus.status.endorsements.statusCode = StatusCode.AVAILABLE;
      when('the class is instanciated', () => {
        then('should return a string "Incomplete"', () => {
          expect(section.status).toBe('Incomplete');
        });
      });
    });

    given('endorsements StatusCode is COMPLETED', () => {
      mockProfileStatus.status.endorsements.statusCode = StatusCode.COMPLETED;
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
        then('the user navigate to endorsements page', () => {
          expect(router.navigate).toHaveBeenCalledWith([
            '///organization-info/endorsements',
          ]);
        });
      });
    });
  });
});
