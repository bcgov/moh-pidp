/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import { ActivatedRoute, Router } from '@angular/router';

import {
  randEmail,
  randFirstName,
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

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import { PartyService } from '@app/core/party/party.service';
import { DocumentService } from '@app/core/services/document.service';

import { AlertCode } from './enums/alert-code.enum';
import { StatusCode } from './enums/status-code.enum';
import { PortalResource } from './portal-resource.service';
import { PortalPage } from './portal.page';
import { PortalService } from './portal.service';
import { IPortalSection } from './sections/classes';

describe('PortalPage', () => {
  let component: PortalPage;
  let partyServiceSpy: Spy<PartyService>;
  let portalResourceSpy: Spy<PortalResource>;
  let portalServiceSpy: Spy<PortalService>;
  let router: Router;

  const mockActivatedRoute = {
    snapshot: {
      data: {
        title: randTextRange({ min: 1, max: 4 }),
      },
    },
  };

  const mockProfileStatusResponse = {
    alerts: [AlertCode.TRANSIENT_ERROR],
    status: {
      demographics: {
        firstName: randFirstName(),
        lastName: randLastName(),
        email: randEmail(),
        phone: randPhoneNumber(),
        statusCode: StatusCode.AVAILABLE,
      },
      collegeCertification: {
        collegeCode: randNumber(),
        licenceNumber: randText(),
        statusCode: StatusCode.AVAILABLE,
      },
      userAccessAgreement: { statusCode: StatusCode.AVAILABLE },
      saEforms: { statusCode: StatusCode.AVAILABLE },
      hcim: { statusCode: StatusCode.AVAILABLE },
      sitePrivacySecurityChecklist: { statusCode: StatusCode.AVAILABLE },
      complianceTraining: { statusCode: StatusCode.AVAILABLE },
      transactions: { statusCode: StatusCode.AVAILABLE },
    },
  };

  beforeEach(() => {
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
        provideAutoSpy(DocumentService),
      ],
    });

    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    component = TestBed.inject(PortalPage);
    partyServiceSpy = TestBed.inject<any>(PartyService);
    portalResourceSpy = TestBed.inject<any>(PortalResource);
    portalServiceSpy = TestBed.inject<any>(PortalService);
    router = TestBed.inject(Router);
  });

  describe('INIT', () => {
    given('partyId exists', () => {
      const partyId = randNumber({ min: 1 });
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
      portalResourceSpy.getProfileStatus.nextWith(mockProfileStatusResponse);
      portalServiceSpy.accessorSpies.getters.alerts.mockReturnValue(
        mockProfileStatusResponse.alerts
      );

      when('the component is initialized', () => {
        expect(component.completedProfile).toEqual(false);
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
          expect(component.completedProfile).toEqual(
            portalServiceSpy.completedProfile
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
      component.ngOnInit();

      when('the card action method is invoked', () => {
        const performAction = (): void => {
          return;
        };
        const performActionSpy =
          createFunctionSpy<typeof performAction>('performAction');

        const section = {
          key: 'demographics',
          type: 'profile',
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
