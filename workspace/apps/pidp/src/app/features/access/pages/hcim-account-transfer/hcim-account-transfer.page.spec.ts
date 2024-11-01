/* eslint-disable @typescript-eslint/no-explicit-any */
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

import {
  randNumber,
  randPassword,
  randTextRange,
  randUserName,
} from '@ngneat/falso';
import { Spy, createSpyFromClass, provideAutoSpy } from 'jest-auto-spies';

import { PartyService } from '@app/core/party/party.service';
import { FormUtilsService } from '@app/core/services/form-utils.service';
import { LoggerService } from '@app/core/services/logger.service';

import {
  HcimAccountTransferResource,
  HcimAccountTransferStatusCode,
} from './hcim-account-transfer-resource.service';
import { HcimAccountTransfer } from './hcim-account-transfer.model';
import { HcimAccountTransferPage } from './hcim-account-transfer.page';

describe('HcimAccountTransferPage', () => {
  let component: HcimAccountTransferPage;
  let partyServiceSpy: Spy<PartyService>;
  let hcimAccounTransferResourceSpy: Spy<HcimAccountTransferResource>;
  let formUtilsServiceSpy: Spy<FormUtilsService>;
  let router: Router;

  let mockActivatedRoute: { snapshot: any };
  let mockCredentials: HcimAccountTransfer;

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
        HcimAccountTransferPage,
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
        provideAutoSpy(HcimAccountTransferResource),
        provideAutoSpy(FormUtilsService),
        provideAutoSpy(LoggerService),
        provideAutoSpy(Router),
      ],
    });

    router = TestBed.inject(Router);
    component = TestBed.inject(HcimAccountTransferPage);
    partyServiceSpy = TestBed.inject<any>(PartyService);
    hcimAccounTransferResourceSpy = TestBed.inject<any>(
      HcimAccountTransferResource,
    );
    formUtilsServiceSpy = TestBed.inject<any>(FormUtilsService);

    mockCredentials = {
      ldapUsername: randUserName(),
      ldapPassword: randPassword(),
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
      component.formState.form.patchValue(mockCredentials);

      when('no validation errors exist', () => {
        formUtilsServiceSpy.checkValidity.mockReturnValue(true);
        const response = {
          statusCode: HcimAccountTransferStatusCode.ACCESS_GRANTED,
          remainingAttempts: randNumber({ min: 1, max: 3 }),
        };
        hcimAccounTransferResourceSpy.requestAccess
          .mustBeCalledWith(partyId, mockCredentials)
          .nextWith(response);
        component.onSubmit();

        then('access request will be made', () => {
          expect(component.completed).toBe(true);
          expect(component.accessRequestStatusCode).toBe(response.statusCode);
          expect(component.loginAttempts).toBe(
            component.maxLoginAttempts - response.remainingAttempts,
          );
        });
      });
    });

    given('a form submission', () => {
      const partyId = randNumber({ min: 1 });
      component.completed = false;
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
      component.formState.form.patchValue(mockCredentials);

      when('validation errors exist', () => {
        formUtilsServiceSpy.checkValidity.mockReturnValue(false);
        component.onSubmit();

        then('access request will not be made', () => {
          expect(component.completed).toBe(false);
          expect(component.accessRequestStatusCode).toBeUndefined();
          expect(component.loginAttempts).toBe(0);
        });
      });
    });
  });
});
