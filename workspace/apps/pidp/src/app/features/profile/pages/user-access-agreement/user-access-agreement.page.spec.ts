import { TestBed } from '@angular/core/testing';
import { ActivatedRoute, Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

import { randFirstName, randFullName, randLastName, randPastDate, randTextRange, randUserName } from '@ngneat/falso';
import { Spy, provideAutoSpy } from 'jest-auto-spies';

import { UserAccessAgreementPage } from './user-access-agreement.page';
import { AccessTokenService } from '@app/features/auth/services/access-token.service';
import { AccessTokenParsed } from '@app/features/auth/models/access-token-parsed.model';
import { IdentityProvider } from '@app/features/auth/enums/identity-provider.enum';

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
      imports: [RouterTestingModule],
      providers: [
        UserAccessAgreementPage,
        {
          provide: ActivatedRoute,
          useValue: mockActivatedRoute,
        },
        provideAutoSpy(Router),
        provideAutoSpy(AccessTokenService)
      ],
    });

    accessTokenServiceSpy = TestBed.inject<any>(AccessTokenService);
    mockAccessTokenParsed = {
      "allowed-origins": [],
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
