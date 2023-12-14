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
    given('StatusCode is AVAILABLE', () => {
      mockProfileStatus.status.administratorInfo.statusCode =
        StatusCode.AVAILABLE;
      when('the class is instanciated', () => {
        then('hint should return a text', () => {
          expect(section.hint).toBe('1 minute to complete');
        });
      });
    });

    given('StatusCode is COMPLETED', () => {
      mockProfileStatus.status.administratorInfo.statusCode =
        StatusCode.COMPLETED;
      when('the class is instanciated', () => {
        then('hint should return an empty text', () => {
          expect(section.hint).toBe('');
        });
      });
    });
  });
});
