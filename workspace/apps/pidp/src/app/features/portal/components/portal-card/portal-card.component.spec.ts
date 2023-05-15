import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RouterTestingModule } from '@angular/router/testing';

import { PortalCardComponent } from './portal-card.component';
import { PrimaryCareRosteringPortalSection } from '../../state/access/primary-care-rostering-portal-section.class';
import { ProfileStatus } from '../../models/profile-status.model';
import { randEmail, randFirstName, randFullName, randLastName, randNumber, randPhoneNumber } from '@ngneat/falso';
import { StatusCode } from '../../enums/status-code.enum';
import { Router } from '@angular/router';
import { provideAutoSpy } from 'jest-auto-spies';
import { By } from '@angular/platform-browser';

describe('PortalCardComponent', () => {
  let component: PortalCardComponent;
  let fixture: ComponentFixture<PortalCardComponent>;
  let router: Router;
  let mockProfileStatus: ProfileStatus;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      declarations: [PortalCardComponent],
      providers: [provideAutoSpy(Router)],
    })
      .compileComponents();

    router = TestBed.inject(Router);

    fixture = TestBed.createComponent(PortalCardComponent);
    component = fixture.componentInstance;

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
        },
        administratorInfo: {
          email: randEmail(),
          statusCode: StatusCode.AVAILABLE,
        },
        organizationDetails: { statusCode: StatusCode.AVAILABLE },
        facilityDetails: { statusCode: StatusCode.AVAILABLE },
        endorsements: { statusCode: StatusCode.AVAILABLE },
        userAccessAgreement: { statusCode: StatusCode.AVAILABLE },
        saEforms: { statusCode: StatusCode.AVAILABLE, incorrectLicenceType: false },
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
        primaryCareRostering: { statusCode: StatusCode.NOT_AVAILABLE },
      },
    };
  });

  describe('Primary Rostering Card', () => {
    given('the status code is NOT_AVAILABLE (locked in the backend)', () => {
      component.section = new PrimaryCareRosteringPortalSection(mockProfileStatus);

      when('the component has been initialized', () => {
        fixture.detectChanges();

        then('should hide the "Learn more" and "Visit" buttons.', () => {
          expect(component.showLearnMore).toBeFalsy();
          expect(component.showVisit).toBeTruthy();
          expect(component.section.action.disabled).toBeTruthy();
          expect(component.showCompleted).toBeFalsy();

          const linkVisit = fixture.debugElement.query(By.css('a'));
          expect(linkVisit).toBeNull();
        });
      });
    });

    given('the status code is AVAILABLE (Incomplete in the backend)', () => {
      mockProfileStatus.status.primaryCareRostering.statusCode = StatusCode.AVAILABLE;
      component.section = new PrimaryCareRosteringPortalSection(mockProfileStatus);

      when('the component has been initialized', () => {
        fixture.detectChanges();

        then('the "Learn more" button should be hidden and the "Visit" button shown', () => {
          expect(component.showLearnMore).toBeFalsy();
          expect(component.showVisit).toBeTruthy();
          expect(component.section.action.disabled).toBeFalsy();
          expect(component.showCompleted).toBeFalsy();

          const linkVisit = fixture.debugElement.query(By.css('a'));
          expect(linkVisit).not.toBeNull();
          expect(linkVisit.attributes.href).toEqual('https://news.gov.bc.ca/releases/2022HLTH0212-001619');
        });
      });
    });
  });
});
