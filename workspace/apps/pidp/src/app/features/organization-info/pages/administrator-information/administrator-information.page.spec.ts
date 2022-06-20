/* eslint-disable @typescript-eslint/no-explicit-any */
import { HttpStatusCode } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

import { randEmail, randNumber, randTextRange } from '@ngneat/falso';
import { Spy, createSpyFromClass, provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import { PartyService } from '@app/core/party/party.service';
import { FormUtilsService } from '@app/core/services/form-utils.service';
import { LoggerService } from '@app/core/services/logger.service';

import { AdministratorInformationResource } from './administrator-information-resource.service';
import { AdministratorInformation } from './administrator-information.model';
import { AdministratorInformationPage } from './administrator-information.page';

describe('AdministratorInformationPage', () => {
  let component: AdministratorInformationPage;
  let partyServiceSpy: Spy<PartyService>;
  let administratorInfoResourceSpy: Spy<AdministratorInformationResource>;
  let formUtilsServiceSpy: Spy<FormUtilsService>;
  let router: Router;

  let mockActivatedRoute: { snapshot: any };
  let mockForm: AdministratorInformation;

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
        AdministratorInformationPage,
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
        provideAutoSpy(AdministratorInformationResource),
        provideAutoSpy(FormUtilsService),
        provideAutoSpy(LoggerService),
        provideAutoSpy(Router),
      ],
    });

    router = TestBed.inject(Router);
    component = TestBed.inject(AdministratorInformationPage);
    partyServiceSpy = TestBed.inject<any>(PartyService);
    administratorInfoResourceSpy = TestBed.inject<any>(
      AdministratorInformationResource
    );
    formUtilsServiceSpy = TestBed.inject<any>(FormUtilsService);

    mockForm = {
      email: randEmail(),
    };
  });

  describe('INIT', () => {
    given('partyId exists', () => {
      const partyId = randNumber({ min: 1 });
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
      administratorInfoResourceSpy.get.nextOneTimeWith(mockForm);

      when('resource request resolved', () => {
        component.ngOnInit();

        then('it should GET party organization details', () => {
          expect(router.navigate).not.toHaveBeenCalled();
          expect(administratorInfoResourceSpy.get).toHaveBeenCalledTimes(1);
          expect(administratorInfoResourceSpy.get).toHaveBeenCalledWith(
            partyId
          );
        });
      });
    });

    given('partyId exists', () => {
      const partyId = randNumber({ min: 1 });
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
      administratorInfoResourceSpy.get.nextWithValues([
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

  describe('METHOD: onSubmit', () => {
    given('a form submission', () => {
      const partyId = randNumber({ min: 1 });
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
      component.formState.form.patchValue(mockForm);

      when('no validation errors exist', () => {
        formUtilsServiceSpy.checkValidity.mockReturnValue(true);
        administratorInfoResourceSpy.update
          .mustBeCalledWith(partyId, mockForm)
          .nextWith(void 0);
        component.onSubmit();

        then(
          'access administrator will be updated and router navigate to root route',
          () => {
            expect(router.navigate).toHaveBeenCalled();
          }
        );
      });
    });

    given('a form submission', () => {
      const partyId = randNumber({ min: 1 });
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
      component.formState.form.patchValue(mockForm);

      when('validation errors exist', () => {
        formUtilsServiceSpy.checkValidity.mockReturnValue(false);
        component.onSubmit();

        then(
          'access administrator should not be updated and router not navigate',
          () => {
            expect(router.navigate).not.toHaveBeenCalled();
          }
        );
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
