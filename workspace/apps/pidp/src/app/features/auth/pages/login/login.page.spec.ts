/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import { MatDialogModule } from '@angular/material/dialog';
import { ActivatedRoute, ParamMap, convertToParamMap } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

import { randTextRange } from '@ngneat/falso';
import { Spy, provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import { DocumentService } from '@app/core/services/document.service';

import { AuthService } from '../../services/auth.service';
import { LoginPage, LoginPageRouteData } from './login.page';
import {
  ClientLogsService,
  MicrosoftLogLevel,
} from '@app/core/services/client-logs.service';
import { ViewportService } from '@bcgov/shared/ui';
import { of } from 'rxjs';

describe('LoginPage', () => {
  let component: LoginPage;

  let clientLogsServiceSpy: Spy<ClientLogsService>;

  let mockActivatedRoute: {
    snapshot: {
      queryParamMap?: ParamMap;
      data: any;
    };
  };

  beforeEach(() => {
    mockActivatedRoute = {
      snapshot: {
        queryParamMap: convertToParamMap({}),
        data: {
          loginPageData: {
            title: randTextRange({ min: 1, max: 4 }),
            isAdminLogin: true,
          } as LoginPageRouteData,
        },
      },
    };

    TestBed.configureTestingModule({
      imports: [RouterTestingModule, MatDialogModule],
      providers: [
        LoginPage,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        {
          provide: ActivatedRoute,
          useValue: mockActivatedRoute,
        },
        provideAutoSpy(AuthService),
        provideAutoSpy(ClientLogsService),
        provideAutoSpy(DocumentService),
        ViewportService,
      ],
    }).compileComponents();

    component = TestBed.inject(LoginPage);

    clientLogsServiceSpy = TestBed.inject<any>(ClientLogsService);
  });

  describe('INIT', () => {
    given('an endorsement-token in the URL', () => {
      mockActivatedRoute.snapshot.queryParamMap = convertToParamMap({
        'endorsement-token': '731c0b67-a475-4068-94e3-367e37a508ec',
      });

      clientLogsServiceSpy.createClientLog.mockReturnValue(of(void 0));

      when('the component has been initialized', () => {
        component.ngOnInit();

        then('a log object was sent to the endpoint "client-logs"', () => {
          expect(clientLogsServiceSpy.createClientLog).toHaveBeenCalledWith({
            message: `A user has landed on the login page with the endorsement request`,
            logLevel: MicrosoftLogLevel.INFORMATION,
            additionalInformation: '731c0b67-a475-4068-94e3-367e37a508ec',
          });
        });
      });
    });

    given('no endorsement-token in the URL', () => {
      when('the component has been initialized', () => {
        component.ngOnInit();

        then('a log object was sent to the endpoint "client-logs"', () => {
          expect(clientLogsServiceSpy.createClientLog).not.toHaveBeenCalled();
        });
      });
    });
  });
});
