import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

import {
  randEmail,
  randFirstName,
  randFullName,
  randLastName,
  randNumber,
  randPhoneNumber,
} from '@ngneat/falso';
import { provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import { AuthService } from '@app/features/auth/services/auth.service';

import { StatusCode } from '../../enums/status-code.enum';
import { ProfileStatus } from '../../models/profile-status.model';
import { PrimaryCareRosteringPortalSection } from '../../state/access/primary-care-rostering-portal-section.class';
import { PortalCardComponent } from './portal-card.component';

describe('PortalCardComponent', () => {
  let component: PortalCardComponent;
  let fixture: ComponentFixture<PortalCardComponent>;
  let router: Router;
  let mockProfileStatus: ProfileStatus;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      declarations: [PortalCardComponent],
      providers: [
        provideAutoSpy(Router),
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        provideAutoSpy(AuthService),
      ],
    }).compileComponents();

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
        primaryCareRostering: { statusCode: StatusCode.NOT_AVAILABLE },
      },
    };
  });

  describe('Primary Rostering Card', () => {
    given('the status code is NOT_AVAILABLE (locked in the backend)', () => {
      component.section = new PrimaryCareRosteringPortalSection(
        mockProfileStatus
      );

      when('the component has been initialized', () => {
        fixture.detectChanges();

        then('should hide the "Learn more" and "Visit" buttons.', () => {
          expect(component.showLearnMore).toBeFalsy();
          expect(component.showVisit).toBeTruthy();
          expect(component.section.action.disabled).toBeTruthy();
          expect(component.showCompleted).toBeFalsy();
        });
      });
    });

    given('the status code is AVAILABLE (Incomplete in the backend)', () => {
      mockProfileStatus.status.primaryCareRostering.statusCode =
        StatusCode.AVAILABLE;
      component.section = new PrimaryCareRosteringPortalSection(
        mockProfileStatus
      );

      when('the component has been initialized', () => {
        fixture.detectChanges();

        then(
          'the "Learn more" button should be hidden and the "Visit" button shown',
          () => {
            expect(component.showLearnMore).toBeFalsy();
            expect(component.showVisit).toBeTruthy();
            expect(component.section.action.disabled).toBeFalsy();
            expect(component.showCompleted).toBeFalsy();

            const linkVisit = fixture.debugElement.query(By.css('button'));
            expect(linkVisit).not.toBeNull();
          }
        );
      });
    });
  });
});
