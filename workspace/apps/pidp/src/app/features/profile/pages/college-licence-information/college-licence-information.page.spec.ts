/* eslint-disable @typescript-eslint/no-explicit-any */
import { HttpStatusCode } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

import { randNumber, randTextRange } from '@ngneat/falso';
import { Spy, createSpyFromClass, provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import { AuthorizedUserService } from '@app/features/auth/services/authorized-user.service';

import { PartyService } from '@core/party/party.service';
import { FormUtilsService } from '@core/services/form-utils.service';
import { LoggerService } from '@core/services/logger.service';

import { CollegeLicenceInformationResource } from './college-licence-information-resource.service';
import { CollegeLicenceInformation } from './college-licence-information.model';
import { CollegeLicenceInformationPage } from './college-licence-information.page';

describe('CollegeLicenceInformationPage', () => {
  let component: CollegeLicenceInformationPage;
  let partyServiceSpy: Spy<PartyService>;
  let collegeLicenceInfoResourceSpy: Spy<CollegeLicenceInformationResource>;
  let formUtilsServiceSpy: Spy<FormUtilsService>;
  let router: Router;

  let mockActivatedRoute: { snapshot: any };
  let mockParty: CollegeLicenceInformation;

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
        CollegeLicenceInformationPage,
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
        provideAutoSpy(CollegeLicenceInformationResource),
        provideAutoSpy(FormUtilsService),
        provideAutoSpy(AuthorizedUserService),
        provideAutoSpy(LoggerService),
        provideAutoSpy(Router),
      ],
    });

    router = TestBed.inject(Router);
    component = TestBed.inject(CollegeLicenceInformationPage);
    partyServiceSpy = TestBed.inject<any>(PartyService);
    collegeLicenceInfoResourceSpy = TestBed.inject<any>(
      CollegeLicenceInformationResource
    );
    formUtilsServiceSpy = TestBed.inject<any>(FormUtilsService);

    mockParty = {
      collegeCode: randNumber(),
      licenceNumber: randTextRange({ min: 1, max: 6 }),
    };
  });

  describe('INIT', () => {
    given('partyId exists', () => {
      const partyId = randNumber({ min: 1 });
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
      collegeLicenceInfoResourceSpy.get.nextOneTimeWith(mockParty);

      when('resource request resolved', () => {
        component.ngOnInit();

        then('should GET party college licence information', () => {
          expect(router.navigate).not.toHaveBeenCalled();
          expect(collegeLicenceInfoResourceSpy.get).toHaveBeenCalledTimes(1);
          expect(collegeLicenceInfoResourceSpy.get).toHaveBeenCalledWith(
            partyId
          );
        });
      });
    });

    given('partyId exists', () => {
      const partyId = randNumber({ min: 1 });
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
      collegeLicenceInfoResourceSpy.get.nextWithValues([
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
      component.formState.form.patchValue(mockParty);

      when('no validation errors exist', () => {
        formUtilsServiceSpy.checkValidity.mockReturnValue(true);
        collegeLicenceInfoResourceSpy.update
          .mustBeCalledWith(partyId, mockParty)
          .nextWith(void 0);
        component.onSubmit();

        then(
          'college info will be updated and router navigate to root route',
          () => {
            expect(router.navigate).toHaveBeenCalled();
          }
        );
      });
    });

    given('a form submission', () => {
      const partyId = randNumber({ min: 1 });
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
      component.formState.form.patchValue(mockParty);
      when('validation errors exist', () => {
        formUtilsServiceSpy.checkValidity.mockReturnValue(false);
        component.onSubmit();

        then(
          'college licence information should not be updated and router not navigate',
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
