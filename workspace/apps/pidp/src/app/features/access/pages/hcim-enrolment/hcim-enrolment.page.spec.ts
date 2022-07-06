/* eslint-disable @typescript-eslint/no-explicit-any */
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

import { randBoolean, randNumber, randTextRange } from '@ngneat/falso';
import { Spy, createSpyFromClass, provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import { PartyService } from '@app/core/party/party.service';
import { FormUtilsService } from '@app/core/services/form-utils.service';
import { LoggerService } from '@app/core/services/logger.service';

import {
  HcimEnrolmentResource,
  HcimEnrolmentStatusCode,
} from './hcim-enrolment-resource.service';
import { HcimEnrolment } from './hcim-enrolment.model';
import { HcimEnrolmentPage } from './hcim-enrolment.page';

describe('HcimEnrolmentComponent', () => {
  let component: HcimEnrolmentPage;
  let partyServiceSpy: Spy<PartyService>;
  let hcimEnrolmentResourceSpy: Spy<HcimEnrolmentResource>;
  let formUtilsServiceSpy: Spy<FormUtilsService>;
  let router: Router;

  let mockActivatedRoute: { snapshot: any };
  let mockForm: HcimEnrolment;

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
      imports: [
        HttpClientTestingModule,
        MatDialogModule,
        ReactiveFormsModule,
        RouterTestingModule,
      ],
      providers: [
        HcimEnrolmentPage,
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
        provideAutoSpy(HcimEnrolmentResource),
        provideAutoSpy(FormUtilsService),
        provideAutoSpy(LoggerService),
        provideAutoSpy(Router),
      ],
    });

    router = TestBed.inject(Router);
    component = TestBed.inject(HcimEnrolmentPage);
    partyServiceSpy = TestBed.inject<any>(PartyService);
    hcimEnrolmentResourceSpy = TestBed.inject<any>(HcimEnrolmentResource);
    formUtilsServiceSpy = TestBed.inject<any>(FormUtilsService);

    mockForm = {
      managesTasks: randBoolean(),
      modifiesPhns: randBoolean(),
      recordsNewborns: randBoolean(),
      searchesIdentifiers: randBoolean(),
    };
  });

  describe('INIT', () => {
    given('partyId exists and access request completion is not null', () => {
      const partyId = randNumber({ min: 1 });
      component.completed = false;
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);

      when('resource request resolved', () => {
        component.ngOnInit();

        then('it should not route to root', () => {
          expect(router.navigate).not.toHaveBeenCalled();
        });
      });
    });

    given('partyId exists and access request completion is null', () => {
      const partyId = randNumber({ min: 1 });
      component.completed = null;
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);

      when('resource request resolved', () => {
        component.ngOnInit();

        then('it should route to root', () => {
          const rootRoute = mockActivatedRoute.snapshot.data.routes.root;
          expect(router.navigate).toHaveBeenCalledWith([rootRoute]);
        });
      });
    });

    given('partyId does not exists', () => {
      const partyId = null;
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);

      when('resource request resolved', () => {
        component.ngOnInit();

        then('it should route to root', () => {
          const rootRoute = mockActivatedRoute.snapshot.data.routes.root;
          expect(router.navigate).toHaveBeenCalledWith([rootRoute]);
        });
      });
    });
  });

  describe('METHOD: onSubmit', () => {
    given('a form submission', () => {
      const partyId = randNumber({ min: 1 });
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
      component.formState.form.patchValue(mockForm);

      when('no validation errors exist', () => {
        formUtilsServiceSpy.checkValidity.mockReturnValue(true);
        const response = {
          statusCode: HcimEnrolmentStatusCode.ACCESS_GRANTED,
        };
        hcimEnrolmentResourceSpy.requestAccess
          .mustBeCalledWith(partyId, mockForm)
          .nextWith(response);
        component.onSubmit();

        then('access request will be made', () => {
          expect(component.completed).toBe(true);
          expect(component.accessRequestStatusCode).toBe(response.statusCode);
        });
      });
    });

    given('a form submission', () => {
      const partyId = randNumber({ min: 1 });
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
      component.formState.form.patchValue(mockForm);

      when('validation errors exist', () => {
        formUtilsServiceSpy.checkValidity.mockReturnValue(false);
        component.onSubmit();

        then('access request will not be made', () => {
          expect(component.completed).toBe(false);
          expect(component.accessRequestStatusCode).toBeUndefined();
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
