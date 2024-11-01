/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import {
  MatDialog,
  MatDialogModule,
  MatDialogRef,
} from '@angular/material/dialog';
import { ActivatedRoute, ParamMap, convertToParamMap } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

import { of } from 'rxjs';

import { randTextRange } from '@ngneat/falso';
import { Spy, provideAutoSpy } from 'jest-auto-spies';

import { ViewportService } from '@bcgov/shared/ui';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import {
  ClientLogsService,
  MicrosoftLogLevel,
} from '@app/core/services/client-logs.service';
import { DocumentService } from '@app/core/services/document.service';
import { LoggerService } from '@app/core/services/logger.service';

import { IdentityProvider } from '../../enums/identity-provider.enum';
import { AuthService } from '../../services/auth.service';
import { LoginResource } from './login-resource.service';
import { LoginPage, LoginPageRouteData } from './login.page';

describe('LoginPage', () => {
  let component: LoginPage;

  let clientLogsServiceSpy: Spy<ClientLogsService>;
  let authServiceSpy: Spy<AuthService>;
  let matDialogSpy: Spy<MatDialog>;
  let loginResourceSpy: Spy<LoginResource>;
  let mockActivatedRoute: {
    snapshot: {
      queryParamMap?: ParamMap;
      data: any;
      routeConfig?: any;
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
        provideAutoSpy(LoginResource),
        provideAutoSpy(LoggerService),
        ViewportService,
      ],
    }).compileComponents();

    component = TestBed.inject(LoginPage);

    clientLogsServiceSpy = TestBed.inject<any>(ClientLogsService);
    authServiceSpy = TestBed.inject<any>(AuthService);
    matDialogSpy = TestBed.inject<any>(MatDialog);
    loginResourceSpy = TestBed.inject<any>(LoginResource);

    loginResourceSpy.findBanners.mockReturnValue(
      of([
        {
          header: 'test',
          body: 'test',
          component: 'test',
          status: '2',
        },
      ]),
    );
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
            message: `A user has landed on the login page with an endorsement request`,
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
              message: `A user has clicked on the login button with an endorsement request and the identity provider "${idpHint}"`,
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
              message: `A user has clicked on the login button with an endorsement request and the identity provider "${idpHint}"`,
              logLevel: MicrosoftLogLevel.INFORMATION,
              additionalInformation: '731c0b67-a475-4068-94e3-367e37a508ec',
            });
            expect(authServiceSpy.login).toHaveBeenCalledWith({
              idpHint: idpHint,
              redirectUri:
                APP_DI_CONFIG.applicationUrl +
                '?endorsement-token=731c0b67-a475-4068-94e3-367e37a508ec',
            });
          });
        });
      },
    );

    given('no endorsement-token and admin path', () => {
      mockActivatedRoute.snapshot.routeConfig = { path: 'admin' };
      const idpHint = IdentityProvider.BCSC;
      clientLogsServiceSpy.createClientLog.mockReturnValue(of(void 0));
      matDialogSpy.open.mockReturnValue({
        afterClosed: () => of(true),
      } as MatDialogRef<typeof component>);

      component.ngOnInit();

      when('the method is called', () => {
        component.onLogin(idpHint);

        then(
          'the log object was not sent and the user is redirectes to the admin path',
          () => {
            expect(clientLogsServiceSpy.createClientLog).not.toHaveBeenCalled();
            expect(authServiceSpy.login).toHaveBeenCalledWith({
              idpHint: idpHint,
              redirectUri: 'http://localhost:4200/admin',
            });
          },
        );
      });
    });

    given(
      'the user logs in with BCSC and clicks the "Cancel" button in the Dialog box',
      () => {
        const idpHint = IdentityProvider.BCSC;
        clientLogsServiceSpy.createClientLog.mockReturnValue(of(void 0));
        matDialogSpy.open.mockReturnValue({
          afterClosed: () => of(false),
        } as MatDialogRef<typeof component>);

        component.ngOnInit();

        when('the method is called', () => {
          component.onLogin(idpHint);

          then('the user stays on the page and nothing is done', () => {
            expect(clientLogsServiceSpy.createClientLog).not.toHaveBeenCalled();
            expect(authServiceSpy.login).not.toHaveBeenCalled();
          });
        });
      },
    );
  });

  describe('METHOD: onShowOtherLoginOptions', () => {
    given('"other login options" is collapsed', () => {
      component.showOtherLoginOptions = false;
      expect(component.otherLoginOptionsIcon).toBe('add_box');

      when('the method is called', () => {
        component.onShowOtherLoginOptions();

        then(
          '"other login options" is expanded and displays a collapse icon',
          () => {
            expect(component.showOtherLoginOptions).toBeTruthy();
            expect(component.otherLoginOptionsIcon).toBe(
              'indeterminate_check_box',
            );
          },
        );
      });
    });

    given('"other login options" is expanded', () => {
      component.showOtherLoginOptions = true;
      expect(component.otherLoginOptionsIcon).toBe('indeterminate_check_box');

      when('the method is called', () => {
        component.onShowOtherLoginOptions();

        then(
          '"other login options" is collapsed and displays an expand icon',
          () => {
            expect(component.showOtherLoginOptions).toBeFalsy();
            expect(component.otherLoginOptionsIcon).toBe('add_box');
          },
        );
      });
    });
  });
});
