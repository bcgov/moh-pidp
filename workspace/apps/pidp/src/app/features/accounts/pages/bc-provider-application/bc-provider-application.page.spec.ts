/* eslint-disable @typescript-eslint/no-explicit-any */
import { HttpClient } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';

import { randNumber, randTextRange } from '@ngneat/falso';
import { NavigationService } from '@pidp/presentation';
import { Spy, createSpyFromClass, provideAutoSpy } from 'jest-auto-spies';
import Keycloak from 'keycloak-js';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import { PartyService } from '@app/core/party/party.service';
import { FormUtilsService } from '@app/core/services/form-utils.service';
import { AccessRoutes } from '@app/features/access/access.routes';

import { BcProviderApplicationPage } from './bc-provider-application.page';

describe('BcProviderApplicationPage', () => {
  let component: BcProviderApplicationPage;
  let partyServiceSpy: Spy<PartyService>;
  let formUtilsServiceSpy: Spy<FormUtilsService>;
  let navigationServiceSpy: Spy<NavigationService>;
  let router: Router;

  let mockActivatedRoute: { snapshot: any };
  let mockBcProviderForm: { password: string; confirmPassword: string };

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
      imports: [MatDialogModule, ReactiveFormsModule],
      providers: [
        BcProviderApplicationPage,
        { provide: APP_CONFIG, useValue: APP_DI_CONFIG },
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
        provideAutoSpy(HttpClient),
        provideAutoSpy(FormUtilsService),
        provideAutoSpy(Keycloak),
        provideAutoSpy(NavigationService),
        provideAutoSpy(Router),
      ],
    });
    component = TestBed.inject(BcProviderApplicationPage);
    formUtilsServiceSpy = TestBed.inject<any>(FormUtilsService);
    navigationServiceSpy = TestBed.inject<any>(NavigationService);
    partyServiceSpy = TestBed.inject<any>(PartyService);
    router = TestBed.inject(Router);
  });

  describe('FORM', () => {
    given('Form is filled out', () => {
      const partyId = randNumber({ min: 1 });
      mockBcProviderForm = {
        password: 'Password1',
        confirmPassword: 'Password1',
      };
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
      component.formState.form.patchValue(mockBcProviderForm);

      when('no validation errors exist', () => {
        formUtilsServiceSpy.checkValidity.mockReturnValue(true);

        then('enrolment button will be enabled', () => {
          expect(component.isEnrolButtonEnabled).toBe(true);
        });
      });
    });

    given('Form is filled out', () => {
      const partyId = randNumber({ min: 1 });
      mockBcProviderForm = {
        password: 'Password1',
        confirmPassword: 'Password2',
      };
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
      component.formState.form.patchValue(mockBcProviderForm);

      when('validation errors exist', () => {
        formUtilsServiceSpy.checkValidity.mockReturnValue(false);

        then('enrolment button will be enabled', () => {
          expect(component.isEnrolButtonEnabled).toBe(false);
        });
      });
    });
  });

  describe('METHOD: onBack', () => {
    given('user wants to go back to the previous page', () => {
      when('onBack is invoked', () => {
        component.onBack();

        then('router should navigate to root route', () => {
          expect(navigationServiceSpy.navigateToRoot).toHaveBeenCalled();
        });
      });
    });
  });

  describe('METHOD: afterSubmitIsSuccessful', () => {
    given('previousUrl does not contain any route segment', () => {
      component.previousUrl = '';

      when('afterSubmitIsSuccessful is invoked', () => {
        component['afterSubmitIsSuccessful']();

        then('should navigate to root', () => {
          expect(navigationServiceSpy.navigateToRoot).toHaveBeenCalled();
        });
      });
    });

    given('User navigated from HALO', () => {
      component.previousUrl = AccessRoutes.HALO;

      when('afterSubmitIsSuccessful is invoked', () => {
        component['afterSubmitIsSuccessful']();

        then('should navigate back to HALO route', () => {
          expect(router.navigateByUrl).toHaveBeenCalledWith(
            AccessRoutes.routePath(AccessRoutes.HALO),
          );
        });
      });
    });

    given('User navigated from IMMSBC', () => {
      component.previousUrl = AccessRoutes.IMMSBC;

      when('afterSubmitIsSuccessful is invoked', () => {
        component['afterSubmitIsSuccessful']();

        then('should navigate back to IMMSBC route', () => {
          expect(router.navigateByUrl).toHaveBeenCalledWith(
            AccessRoutes.routePath(AccessRoutes.IMMSBC),
          );
        });
      });
    });

    given('User navigated from IVF', () => {
      component.previousUrl = AccessRoutes.IVF;

      when('afterSubmitIsSuccessful is invoked', () => {
        component['afterSubmitIsSuccessful']();

        then('should navigate back to IVF route', () => {
          expect(router.navigateByUrl).toHaveBeenCalledWith(
            AccessRoutes.routePath(AccessRoutes.IVF),
          );
        });
      });
    });

    given('User navigated from PEMCOD', () => {
      component.previousUrl = AccessRoutes.PEMCOD;

      when('afterSubmitIsSuccessful is invoked', () => {
        component['afterSubmitIsSuccessful']();

        then('should navigate back to PEMCOD route', () => {
          expect(router.navigateByUrl).toHaveBeenCalledWith(
            AccessRoutes.routePath(AccessRoutes.PEMCOD),
          );
        });
      });
    });

    given('User navigated from the Provincial Attachment System', () => {
      component.previousUrl = AccessRoutes.PROVINCIAL_ATTACHMENT_SYSTEM;

      when('afterSubmitIsSuccessful is invoked', () => {
        component['afterSubmitIsSuccessful']();

        then('should navigate back to the PAS route', () => {
          expect(router.navigateByUrl).toHaveBeenCalledWith(
            AccessRoutes.routePath(AccessRoutes.PROVINCIAL_ATTACHMENT_SYSTEM),
          );
        });
      });
    });
  });
});
