/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';

import {
  randFirstName,
  randFullName,
  randLastName,
  randPastDate,
  randUserName,
} from '@ngneat/falso';
import { Spy, provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import { IdentityProvider } from '@app/features/auth/enums/identity-provider.enum';
import { AccessTokenParsed } from '@app/features/auth/models/access-token-parsed.model';
import { AccessTokenService } from '@app/features/auth/services/access-token.service';
import { AuthService } from '@app/features/auth/services/auth.service';

import { AdminDashboardComponent } from './admin-dashboard.component';

describe('AdminDashboardComponent', () => {
  let component: AdminDashboardComponent;
  let accessTokenServiceSpy: Spy<AccessTokenService>;

  let mockAccessTokenParsed: AccessTokenParsed;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        AdminDashboardComponent,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        provideAutoSpy(AuthService),
        provideAutoSpy(AccessTokenService),
      ],
    });

    accessTokenServiceSpy = TestBed.inject<any>(AccessTokenService);
    mockAccessTokenParsed = {
      'allowed-origins': [],
      acr: '',
      aud: '',
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

    component = TestBed.inject(AdminDashboardComponent);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
