/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import {
  MatDialog,
  MatDialogModule,
  MatDialogRef,
} from '@angular/material/dialog';
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
import { IdentityProvider } from '../../enums/identity-provider.enum';

describe('LoginPage', () => {
  let component: LoginPage;

  let clientLogsServiceSpy: Spy<ClientLogsService>;
  let matDialogSpy: Spy<MatDialog>;

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
        provideAutoSpy(MatDialog),
        ViewportService,
      ],
    }).compileComponents();

    component = TestBed.inject(LoginPage);

    clientLogsServiceSpy = TestBed.inject<any>(ClientLogsService);
    matDialogSpy = TestBed.inject<any>(MatDialog);
  });

  describe('INIT', () => {
    given('an endorsement-token in the URL', () => {
      mockActivatedRoute.snapshot.queryParamMap = convertToParamMap({
        'endorsement-token': '731c0b67-a475-4068-94e3-367e37a508ec',
      });

      when('the component has been initialized', () => {
        clientLogsServiceSpy.createClientLog.mockReturnValue(of(void 0));

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

  describe('METHOD: onLogin', () => {
    given(
      'an endorsement-token in the URL and the user logs in with IDIR',
      () => {
        mockActivatedRoute.snapshot.queryParamMap = convertToParamMap({
          'endorsement-token': '731c0b67-a475-4068-94e3-367e37a508ec',
        });
        const idpHint = IdentityProvider.IDIR;
        clientLogsServiceSpy.createClientLog.mockReturnValue(of(void 0));
        component.ngOnInit();

        when('the method is called', () => {
          component.onLogin(idpHint);

          then('the log object was sent to the endpoint "client-logs"', () => {
            expect(clientLogsServiceSpy.createClientLog).toHaveBeenCalledTimes(
              2,
            );
            expect(
              clientLogsServiceSpy.createClientLog,
            ).toHaveBeenLastCalledWith({
              message: `A user has clicked on the login button with the endorsement request and the identity provider "${idpHint}"`,
              logLevel: MicrosoftLogLevel.INFORMATION,
              additionalInformation: '731c0b67-a475-4068-94e3-367e37a508ec',
            });
          });
        });
      },
    );

    given(
      'an endorsement-token in the URL and the user logs in with BCSC',
      () => {
        mockActivatedRoute.snapshot.queryParamMap = convertToParamMap({
          'endorsement-token': '731c0b67-a475-4068-94e3-367e37a508ec',
        });
        const idpHint = IdentityProvider.BCSC;
        clientLogsServiceSpy.createClientLog.mockReturnValue(of(void 0));
        matDialogSpy.open.mockReturnValue({
          afterClosed: () => of(true),
        } as MatDialogRef<typeof component>);

        component.ngOnInit();

        when('the method is called', () => {
          component.onLogin(idpHint);

          then('the log object was sent to the endpoint "client-logs"', () => {
            expect(clientLogsServiceSpy.createClientLog).toHaveBeenCalledTimes(
              2,
            );
            expect(
              clientLogsServiceSpy.createClientLog,
            ).toHaveBeenLastCalledWith({
              message: `A user has clicked on the login button with the endorsement request and the identity provider "${idpHint}"`,
              logLevel: MicrosoftLogLevel.INFORMATION,
              additionalInformation: '731c0b67-a475-4068-94e3-367e37a508ec',
            });
            // TODO: expect for private login() function
          });
        });
      },
    );
  });
});
