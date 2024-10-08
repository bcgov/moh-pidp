/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import { FormBuilder } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

import { randTextRange } from '@ngneat/falso';
import { provideAutoSpy } from 'jest-auto-spies';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { FormUtilsService } from '@app/core/services/form-utils.service';
import { LoggerService } from '@app/core/services/logger.service';

import { UserAccessAgreementResource } from './user-access-agreement-resource.service';
import { UserAccessAgreementPage } from './user-access-agreement.page';

describe('UserAccessAgreementPage', () => {
  let component: UserAccessAgreementPage;
  let router: Router;
  let scroll = window.scroll;

  let mockActivatedRoute: { snapshot: any };

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
      imports: [MatDialogModule, RouterTestingModule],
      providers: [
        UserAccessAgreementPage,
        {
          provide: ActivatedRoute,
          useValue: mockActivatedRoute,
        },
        provideAutoSpy(ApiHttpClient),
        provideAutoSpy(FormBuilder),
        provideAutoSpy(FormUtilsService),
        provideAutoSpy(LoggerService),
        provideAutoSpy(Router),
        provideAutoSpy(UserAccessAgreementResource),
      ],
    });

    component = TestBed.inject(UserAccessAgreementPage);
    router = TestBed.inject(Router);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('METHOD: onBack', () => {
    given('user wants to go back to the previous page', () => {
      when('onBack is invoked', () => {
        component.onBack();
        window.scroll = scroll;
        then('router should navigate to root route', () => {
          const rootRoute = mockActivatedRoute.snapshot.data.routes.root;
          expect(router.navigate).toHaveBeenCalledWith([rootRoute]);
        });
      });
    });
  });
});
