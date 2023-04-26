import { TestBed } from '@angular/core/testing';

import { Spy, provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import { AccessTokenService } from '@app/features/auth/services/access-token.service';
import { AuthService } from '@app/features/auth/services/auth.service';

import { PortalDashboardComponent } from './portal-dashboard.component';
import { PortalResource } from '@app/features/portal/portal-resource.service';
import { LookupService } from '@app/modules/lookup/lookup.service';
import { AccessTokenParsed } from '@app/features/auth/models/access-token-parsed.model';
import { IdentityProvider } from '@app/features/auth/enums/identity-provider.enum';
import { randFirstName, randFullName, randLastName, randPastDate, randUserName } from '@ngneat/falso';
import { Role } from '@app/shared/enums/roles.enum';

describe('PortalDashboardComponent', () => {
  let component: PortalDashboardComponent;
  let accessTokenServiceSpy: Spy<AccessTokenService>;
  let mockAccessTokenParsed: AccessTokenParsed;
  
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        PortalDashboardComponent,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        provideAutoSpy(AuthService),
        provideAutoSpy(PortalResource),
        provideAutoSpy(LookupService),
        provideAutoSpy(AccessTokenService),
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
    accessTokenServiceSpy.roles.mockReturnValue([Role.ADMIN, Role.FEATURE_PIDP_DEMO]);

    component = TestBed.inject(PortalDashboardComponent);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
