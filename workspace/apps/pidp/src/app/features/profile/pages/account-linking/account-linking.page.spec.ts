/* eslint-disable @typescript-eslint/no-explicit-any */
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
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
import { Spy, createSpyFromClass, provideAutoSpy } from 'jest-auto-spies';
import { KeycloakService } from 'keycloak-angular';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import { PartyService } from '@app/core/party/party.service';
import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { IdentityProvider } from '@app/features/auth/enums/identity-provider.enum';
import { AccessTokenParsed } from '@app/features/auth/models/access-token-parsed.model';
import { AccessTokenService } from '@app/features/auth/services/access-token.service';
import { AuthorizedUserService } from '@app/features/auth/services/authorized-user.service';

import { AccountLinkingResource } from './account-linking-resource.service';
import { AccountLinkingPage } from './account-linking.page';
import { NavigationService } from '@pidp/presentation';

describe('AccountLinkingPage', () => {
  let component: AccountLinkingPage;
  let accessTokenServiceSpy: Spy<AccessTokenService>;
  let navigationServiceSpy: Spy<NavigationService>;

  let mockAccessTokenParsed: AccessTokenParsed;
  let mockActivatedRoute: { snapshot: any };

  beforeEach(async () => {
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
      imports: [HttpClientTestingModule, RouterTestingModule],
      providers: [
        AccountLinkingPage,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
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
        {
          provide: AuthorizedUserService,
          useValue: createSpyFromClass(AuthorizedUserService, {
            gettersToSpyOn: ['roles'],
          }),
        },
        provideAutoSpy(AccessTokenService),
        provideAutoSpy(AccountLinkingResource),
        provideAutoSpy(ApiHttpClient),
        provideAutoSpy(Router),
        provideAutoSpy(KeycloakService),
        provideAutoSpy(NavigationService)
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
      identity_provider: IdentityProvider.PHSA,
      email_verified: true,
      family_name: randLastName(),
      given_name: randFirstName(),
      name: randFullName(),
      preferred_username: randUserName(),
      birthdate: randPastDate().toString(),
    };

    accessTokenServiceSpy.decodeToken.nextWith(mockAccessTokenParsed);

    component = TestBed.inject(AccountLinkingPage);
    navigationServiceSpy = TestBed.inject<any>(NavigationService);
    navigationServiceSpy.getPreviousUrl();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
