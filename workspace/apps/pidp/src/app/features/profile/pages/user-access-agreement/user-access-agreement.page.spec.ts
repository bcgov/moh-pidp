/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import { FormBuilder } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

import {
  randFirstName,
  randFullName,
  randLastName,
  randPastDate,
  randTextRange,
  randUserName,
} from '@ngneat/falso';
import { Spy, provideAutoSpy } from 'jest-auto-spies';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { FormUtilsService } from '@app/core/services/form-utils.service';
import { LoggerService } from '@app/core/services/logger.service';
import { IdentityProvider } from '@app/features/auth/enums/identity-provider.enum';
import { AccessTokenParsed } from '@app/features/auth/models/access-token-parsed.model';
import { AccessTokenService } from '@app/features/auth/services/access-token.service';

import { UserAccessAgreementResource } from './user-access-agreement-resource.service';
import { UserAccessAgreementPage } from './user-access-agreement.page';

describe('UserAccessAgreementPage', () => {
  let component: UserAccessAgreementPage;
  let accessTokenServiceSpy: Spy<AccessTokenService>;

  let mockAccessTokenParsed: AccessTokenParsed;
  let mockActivatedRoute;

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
        provideAutoSpy(AccessTokenService),
        provideAutoSpy(ApiHttpClient),
        provideAutoSpy(FormBuilder),
        provideAutoSpy(FormUtilsService),
        provideAutoSpy(LoggerService),
        provideAutoSpy(Router),
        provideAutoSpy(UserAccessAgreementResource),
      ],
    });

    accessTokenServiceSpy = TestBed.inject<any>(AccessTokenService);
    mockAccessTokenParsed = {
      'allowed-origins': [],
      acr: '',
      aud: [],
      auth_time: 0,
      azp: '',
      iss: '',
      jti: '',
      scope: '',
      sub: '',
      typ: '',
      // User specific attributes:
      identity_provider: IdentityProvider.IDIR,
      email_verified: true,
      family_name: randLastName(),
      given_name: randFirstName(),
      name: randFullName(),
      preferred_username: randUserName(),
      birthdate: randPastDate().toString(),
    };

    accessTokenServiceSpy.decodeToken.nextWith(mockAccessTokenParsed);

    component = TestBed.inject(UserAccessAgreementPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
