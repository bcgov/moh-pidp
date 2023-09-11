/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import { ActivatedRoute, Router, convertToParamMap } from '@angular/router';

import {
  randEmail,
  randFirstName,
  randFullName,
  randLastName,
  randNumber,
  randPhoneNumber,
  randText,
  randTextRange,
} from '@ngneat/falso';
import {
  Spy,
  createFunctionSpy,
  createSpyFromClass,
  provideAutoSpy,
} from 'jest-auto-spies';
import { KeycloakService } from 'keycloak-angular';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import { PartyService } from '@app/core/party/party.service';
import { ToastService } from '@app/core/services/toast.service';

import { BcProviderEditResource } from '../access/pages/bc-provider-edit/bc-provider-edit-resource.service';
import { BcProviderEditInitialStateModel } from '../access/pages/bc-provider-edit/bc-provider-edit.page';
import { EndorsementsResource } from '../organization-info/pages/endorsements/endorsements-resource.service';
import { AlertCode } from './enums/alert-code.enum';
import { StatusCode } from './enums/status-code.enum';
import { ProfileStatus } from './models/profile-status.model';
import { PortalResource } from './portal-resource.service';
import { PortalPage } from './portal.page';
import { PortalService } from './portal.service';
import { IPortalSection } from './state/portal-section.model';

describe('PortalPage', () => {
  let component: PortalPage;
  let partyServiceSpy: Spy<PartyService>;
  let portalResourceSpy: Spy<PortalResource>;
  let portalServiceSpy: Spy<PortalService>;
  let bcProviderResourceSpy: Spy<BcProviderEditResource>;
  let router: Router;

  let mockActivatedRoute;
  let mockProfileStatusResponse: ProfileStatus;
  let mockBcProviderEditInitialStateResponse: BcProviderEditInitialStateModel;

  beforeEach(() => {
    mockActivatedRoute = {
      snapshot: {
        queryParamMap: convertToParamMap({ 'endorsement-token': '' }),
        data: {
          title: randTextRange({ min: 1, max: 4 }),
        },
      },
    };

    TestBed.configureTestingModule({
      providers: [
        PortalPage,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        {
          provide: ActivatedRoute,
          useValue: mockActivatedRoute,
        },
        {
          provide: PartyService,
          useValue: createSpyFromClass(PartyService, {
            gettersToSpyOn: ['partyId'],
            settersToSpyOn: ['partyId'],
          }),
        },
        provideAutoSpy(Router),
        provideAutoSpy(PortalResource),
        {
          provide: PortalService,
          useValue: createSpyFromClass(PortalService, {
            gettersToSpyOn: ['alerts'],
            settersToSpyOn: ['alerts'],
          }),
        },
        provideAutoSpy(BcProviderEditResource),
        provideAutoSpy(EndorsementsResource),
        provideAutoSpy(KeycloakService),
        provideAutoSpy(ToastService),
      ],
    });

    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    component = TestBed.inject(PortalPage);
    partyServiceSpy = TestBed.inject<any>(PartyService);
    portalResourceSpy = TestBed.inject<any>(PortalResource);
    portalServiceSpy = TestBed.inject<any>(PortalService);
    bcProviderResourceSpy = TestBed.inject<any>(BcProviderEditResource);
    router = TestBed.inject(Router);

    mockProfileStatusResponse = {
      alerts: [AlertCode.TRANSIENT_ERROR],
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
        primaryCareRostering: { statusCode: StatusCode.AVAILABLE },
      },
    };

    mockBcProviderEditInitialStateResponse = {
      bcProviderId: 'Id',
    };
  });

  describe('INIT', () => {
    given('partyId exists', () => {
      const partyId = randNumber({ min: 1 });
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
      portalResourceSpy.getProfileStatus.nextWith(mockProfileStatusResponse);
      bcProviderResourceSpy.get.nextWith(
        mockBcProviderEditInitialStateResponse
      );
      portalServiceSpy.accessorSpies.getters.alerts.mockReturnValue(
        mockProfileStatusResponse.alerts
      );

      when('the component is initialized', () => {
        expect(component.alerts.length).toEqual(0);
        component.ngOnInit();

        then('a request should be made to get the profile status', () => {
          expect(portalResourceSpy.getProfileStatus).toHaveBeenCalledTimes(1);
          expect(portalResourceSpy.getProfileStatus).toHaveBeenCalledWith(
            partyId
          );
          expect(portalServiceSpy.updateState).toHaveBeenCalledWith(
            mockProfileStatusResponse
          );
          expect(component.alerts.length).toEqual(1);
        });
      });
    });
  });

  describe('METHOD: onScrollToAnchor', () => {
    given('the component has been initialized', () => {
      const partyId = randNumber({ min: 1 });
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
      portalResourceSpy.getProfileStatus.nextWith(mockProfileStatusResponse);
      bcProviderResourceSpy.get.nextWith(
        mockBcProviderEditInitialStateResponse
      );
      component.ngOnInit();

      when('the scroll to anchor method is invoked', () => {
        component.onScrollToAnchor();

        then('the router should navigate', () => {
          expect(router.navigate).toHaveBeenCalledWith([], {
            fragment: 'access',
            queryParamsHandling: 'preserve',
          });
        });
      });
    });
  });

  describe('METHOD: onCardAction', () => {
    given('the component has been initialized', () => {
      const partyId = randNumber({ min: 1 });
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
      portalResourceSpy.getProfileStatus.nextWith(mockProfileStatusResponse);
      bcProviderResourceSpy.get.nextWith(
        mockBcProviderEditInitialStateResponse
      );
      component.ngOnInit();

      when('the card action method is invoked', () => {
        const performAction = (): void => {
          return;
        };
        const performActionSpy =
          createFunctionSpy<typeof performAction>('performAction');

        const section = {
          key: 'demographics',
          heading: randText(),
          hint: randText(),
          description: randText(),
          properties: [],
          action: {
            label: '',
            route: '',
            disabled: false,
          },
          statusType: 'success',
          status: randText(),
          performAction: performActionSpy,
        } as IPortalSection;

        component.onCardAction(section);

        then('the section callback should be invoked', () => {
          expect(performActionSpy).toHaveBeenCalledTimes(1);
        });
      });
    });
  });
});
