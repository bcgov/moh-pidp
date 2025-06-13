/* eslint-disable @typescript-eslint/no-explicit-any */
import { HttpStatusCode } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';

import {
  randCity,
  randCountryCode,
  randEmail,
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
import { AuthorizedUserService } from '@app/features/auth/services/authorized-user.service';

import { PersonalInformationResource } from './personal-information-resource.service';
import { PersonalInformation } from './personal-information.model';
import { PersonalInformationPage } from './personal-information.page';

describe('PersonalInformationPage', () => {
  let component: PersonalInformationPage;
  let partyServiceSpy: Spy<PartyService>;
  let personalInfoResourceSpy: Spy<PersonalInformationResource>;
  let formUtilsServiceSpy: Spy<FormUtilsService>;
  let router: Router;

  let mockActivatedRoute: { snapshot: any };
  let mockForm: Omit<PersonalInformation, 'mailingAddress'>;
  let mockParty: PersonalInformation;

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
        MatSnackBarModule,
        ReactiveFormsModule,
        RouterModule.forRoot([]),
      ],
      providers: [
        PersonalInformationPage,
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
        provideAutoSpy(PersonalInformationResource),
        provideAutoSpy(FormUtilsService),
        provideAutoSpy(AuthorizedUserService),
        provideAutoSpy(LoggerService),
        provideAutoSpy(Router),
      ],
    });

    router = TestBed.inject(Router);
    component = TestBed.inject(PersonalInformationPage);
    partyServiceSpy = TestBed.inject<any>(PartyService);
    personalInfoResourceSpy = TestBed.inject<any>(PersonalInformationResource);
    formUtilsServiceSpy = TestBed.inject<any>(FormUtilsService);

    mockForm = {
      preferredFirstName: '',
      preferredMiddleName: '',
      preferredLastName: '',
      phone: `${randNumber({ min: 2000000000, max: 9999999999 })}`,
      email: randEmail(),
    };

    mockParty = {
      mailingAddress: {
        street: randStreetAddress(),
        city: randCity(),
        provinceCode: randStateAbbr(),
        countryCode: randCountryCode(),
        postal: randZipCode(),
      },
      ...mockForm,
    };
  });

  describe('INIT', () => {
    given('partyId exists', () => {
      const partyId = randNumber({ min: 1 });
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
      personalInfoResourceSpy.get.nextOneTimeWith(mockParty);

      when('resource request resolved', () => {
        component.ngOnInit();

        then('it should GET party personal information information', () => {
          expect(router.navigate).not.toHaveBeenCalled();
          expect(personalInfoResourceSpy.get).toHaveBeenCalledTimes(1);
          expect(personalInfoResourceSpy.get).toHaveBeenCalledWith(partyId);
        });
      });
    });

    given('partyId exists', () => {
      const partyId = randNumber({ min: 1 });
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
      personalInfoResourceSpy.get.nextWithValues([
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
        personalInfoResourceSpy.update
          .mustBeCalledWith(partyId, mockForm as PersonalInformation)
          .nextWith(void 0);

        component.onSubmit();

        then('user will be updated and router navigate to root route', () => {
          expect(router.navigate).toHaveBeenCalled();
        });
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
          'party personal information should not be updated and router not navigate',
          () => {
            expect(router.navigate).not.toHaveBeenCalled();
          },
        );
      });
    });
  });

  describe('METHOD: onPreferredNameToggle', () => {
    given('section with toggle for preferred name(s)', () => {
      when('toggle has been checked', () => {
        component.onPreferredNameToggle({ checked: true });

        then('preferred name fields are shown', () => {
          expect(component.hasPreferredName).toEqual(true);
        });
      });
    });

    given('section with toggle for preferred name(s)', () => {
      when('toggle has been unchecked', () => {
        component.onPreferredNameToggle({ checked: false });

        then('preferred name fields are hidden', () => {
          expect(component.hasPreferredName).toEqual(false);
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
