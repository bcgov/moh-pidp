/* eslint-disable @typescript-eslint/no-explicit-any */
import { HttpStatusCode } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

import {
  randCity,
  randCountryCode,
  randNumber,
  randStateAbbr,
  randStreetAddress,
  randTextRange,
  randZipCode,
} from '@ngneat/falso';
import { Spy, createSpyFromClass, provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import { PartyService } from '@app/core/party/party.service';
import { FormUtilsService } from '@app/core/services/form-utils.service';
import { LoggerService } from '@app/core/services/logger.service';

import { WorkAndRoleInformationResource } from './work-and-role-information-resource.service';
import { WorkAndRoleInformation } from './work-and-role-information.model';
import { WorkAndRoleInformationPage } from './work-and-role-information.page';

describe('WorkAndRoleInformationPage', () => {
  let component: WorkAndRoleInformationPage;
  let partyServiceSpy: Spy<PartyService>;
  let workAndRoleInformationResourceSpy: Spy<WorkAndRoleInformationResource>;
  let router: Router;

  let mockActivatedRoute: { snapshot: any };
  let mockForm: Pick<WorkAndRoleInformation, 'jobTitle' | 'facilityName'>;
  let mockParty: WorkAndRoleInformation;

  beforeEach(() => {
    mockActivatedRoute = {
      snapshot: {
        data: {
          title: randTextRange({ min: 1, max: 4 }),
          routes: {
            root: '../../',
          },
        },
      },
    };

    TestBed.configureTestingModule({
      imports: [MatDialogModule, ReactiveFormsModule, RouterTestingModule],
      providers: [
        WorkAndRoleInformationPage,
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
        provideAutoSpy(WorkAndRoleInformationResource),
        provideAutoSpy(FormUtilsService),
        provideAutoSpy(LoggerService),
        provideAutoSpy(Router),
      ],
    });

    component = TestBed.inject(WorkAndRoleInformationPage);
    partyServiceSpy = TestBed.inject<any>(PartyService);
    workAndRoleInformationResourceSpy = TestBed.inject<any>(
      WorkAndRoleInformationResource
    );
    router = TestBed.inject(Router);

    mockForm = {
      jobTitle: randTextRange({ min: 4, max: 15 }),
      facilityName: randTextRange({ min: 4, max: 15 }),
    };

    mockParty = {
      facilityAddress: {
        countryCode: randCountryCode(),
        provinceCode: randStateAbbr(),
        street: randStreetAddress(),
        city: randCity(),
        postal: randZipCode(),
      },
      ...mockForm,
    };
  });

  describe('INIT', () => {
    given('partyId exists', () => {
      const partyId = randNumber({ min: 1 });
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
      workAndRoleInformationResourceSpy.get.nextOneTimeWith(mockParty);

      when('resource request resolved', () => {
        component.ngOnInit();

        then('it should GET party work and role information', () => {
          expect(router.navigate).not.toHaveBeenCalled();
          expect(workAndRoleInformationResourceSpy.get).toHaveBeenCalledTimes(
            1
          );
          expect(workAndRoleInformationResourceSpy.get).toHaveBeenCalledWith(
            partyId
          );
        });
      });
    });

    given('partyId exists', () => {
      const partyId = randNumber({ min: 1 });
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
      workAndRoleInformationResourceSpy.get.nextWithValues([
        {
          errorValue: {
            status: HttpStatusCode.NotFound,
          },
        },
      ]);

      when('resource request rejected', () => {
        component.ngOnInit();

        then('router should navigate to root route', () => {
          const rootRoute = mockActivatedRoute.snapshot.data.routes.root;
          expect(router.navigate).toHaveBeenCalledWith([rootRoute]);
        });
      });
    });

    given('partyId does not exist', () => {
      const partyId = null;
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);

      when('initializing the component', () => {
        component.ngOnInit();

        then('router should navigate to root route', () => {
          const rootRoute = mockActivatedRoute.snapshot.data.routes.root;
          expect(router.navigate).toHaveBeenCalledWith([rootRoute]);
        });
      });
    });
  });

  describe('METHOD: onBack', () => {
    given('user wants to go back to the previous page', () => {
      when('onBack is invoked', () => {
        component.onBack();

        then('router should navigate to root route', () => {
          const rootRoute = mockActivatedRoute.snapshot.data.routes.root;
          expect(router.navigate).toHaveBeenCalledWith([rootRoute]);
        });
      });
    });
  });
});
