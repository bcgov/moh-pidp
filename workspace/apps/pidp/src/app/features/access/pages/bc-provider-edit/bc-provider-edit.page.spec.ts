/* eslint-disable @typescript-eslint/no-explicit-any */
import { HttpClient } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';

import { randNumber, randTextRange } from '@ngneat/falso';
import { NavigationService } from '@pidp/presentation';
import { Spy, createSpyFromClass, provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import { PartyService } from '@app/core/party/party.service';
import { FormUtilsService } from '@app/core/services/form-utils.service';
import { AuthorizedUserService } from '@app/features/auth/services/authorized-user.service';

import { BcProviderEditPage } from './bc-provider-edit.page';

describe('BcProviderEditPage', () => {
  let component: BcProviderEditPage;
  let partyServiceSpy: Spy<PartyService>;
  let formUtilsServiceSpy: Spy<FormUtilsService>;

  let mockActivatedRoute: { snapshot: any };
  let mockBcProviderForm: { newPassword: string; confirmPassword: string };

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
      imports: [MatDialogModule, MatSnackBarModule, ReactiveFormsModule],
      providers: [
        BcProviderEditPage,
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
        provideAutoSpy(NavigationService),
        provideAutoSpy(Router),
        provideAutoSpy(AuthorizedUserService),
      ],
    });
    component = TestBed.inject(BcProviderEditPage);
    formUtilsServiceSpy = TestBed.inject<any>(FormUtilsService);
    partyServiceSpy = TestBed.inject<any>(PartyService);
  });

  describe('FORM', () => {
    given('Form is filled out', () => {
      const partyId = randNumber({ min: 1 });
      mockBcProviderForm = {
        newPassword: 'Password1',
        confirmPassword: 'Password1',
      };
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
      component.formState.form.patchValue(mockBcProviderForm);

      when('no validation errors exist', () => {
        formUtilsServiceSpy.checkValidity.mockReturnValue(true);

        then('enrolment button will be enabled', () => {
          expect(component.isResetButtonEnabled).toBe(true);
        });
      });
    });

    given('Form is filled out', () => {
      const partyId = randNumber({ min: 1 });
      mockBcProviderForm = {
        newPassword: 'Password1',
        confirmPassword: 'Password2',
      };
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
      component.formState.form.patchValue(mockBcProviderForm);

      when('validation errors exist', () => {
        formUtilsServiceSpy.checkValidity.mockReturnValue(false);

        then('enrolment button will be enabled', () => {
          expect(component.isResetButtonEnabled).toBe(false);
        });
      });
    });
  });
});
