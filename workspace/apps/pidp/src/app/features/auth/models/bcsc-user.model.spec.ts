import {
  randEmail,
  randFirstName,
  randFullName,
  randLastName,
  randPastDate,
  randUserName,
} from '@ngneat/falso';

import { IdentityProvider } from '../enums/identity-provider.enum';
import { AccessTokenParsed } from './access-token-parsed.model';
import { BcscResolver, BcscUser } from './bcsc-user.model';
import { BrokerProfile } from './broker-profile.model';

describe('BcscUser', () => {
  let mockAccessTokenParsed: AccessTokenParsed;
  let mockBrokerProfile: BrokerProfile;

  describe('METHOD: constructor', () => {
    given('AccessTokenParsed and BrokerProfile parameters', () => {
      mockAccessTokenParsed = {
        'allowed-origins': [],
        acr: '',
        aud: '',
        auth_time: 0,
        azp: '',
        iss: '',
        jti: '',
        scope: '',
        sub: '685c87f8-ea2e-4efe-9b2e-8ef506110b4d',
        typ: '',
        // User specific attributes:
        identity_provider: IdentityProvider.BCSC,
        email_verified: true,
        family_name: randLastName(),
        given_name: randFirstName(),
        name: randFullName(),
        preferred_username: randUserName(),
        birthdate: randPastDate().toString(),
      };
      mockBrokerProfile = {
        firstName: mockAccessTokenParsed.family_name,
        lastName: mockAccessTokenParsed.given_name,
        birthdate: mockAccessTokenParsed.birthdate,
        username: mockAccessTokenParsed.preferred_username,
        email: randEmail(),
        attributes: {
          birthdate: mockAccessTokenParsed.birthdate,
        },
      };

      when('class is instanciated', () => {
        const bcscUser = new BcscUser({
          accessTokenParsed: mockAccessTokenParsed,
          brokerProfile: mockBrokerProfile,
        });

        then('properties should be filled out', () => {
          expect(bcscUser.identityProvider).toBe(
            mockAccessTokenParsed.identity_provider,
          );
          expect(bcscUser.userId).toBe(mockAccessTokenParsed.sub);
          expect(bcscUser.idpId).toBe(mockBrokerProfile.username);
          expect(bcscUser.firstName).toBe(mockBrokerProfile.firstName);
          expect(bcscUser.lastName).toBe(mockBrokerProfile.lastName);
        });
      });
    });
  });
});

describe('BcscResolver', () => {
  let mockAccessTokenParsed: AccessTokenParsed;
  let mockBrokerProfile: BrokerProfile;

  describe('METHOD: resolve', () => {
    given('userIdentity in parameter', () => {
      mockAccessTokenParsed = {
        'allowed-origins': [],
        acr: '',
        aud: '',
        auth_time: 0,
        azp: '',
        iss: '',
        jti: '',
        scope: '',
        sub: '685c87f8-ea2e-4efe-9b2e-8ef506110b4d',
        typ: '',
        // User specific attributes:
        identity_provider: IdentityProvider.BCSC,
        email_verified: true,
        family_name: randLastName(),
        given_name: randFirstName(),
        name: randFullName(),
        preferred_username: randUserName(),
        birthdate: randPastDate().toString(),
      };
      mockBrokerProfile = {
        firstName: mockAccessTokenParsed.family_name,
        lastName: mockAccessTokenParsed.given_name,
        birthdate: mockAccessTokenParsed.birthdate,
        username: mockAccessTokenParsed.preferred_username,
        email: randEmail(),
        attributes: {
          birthdate: mockAccessTokenParsed.birthdate,
        },
      };
      const bcscResolver = new BcscResolver({
        accessTokenParsed: mockAccessTokenParsed,
        brokerProfile: mockBrokerProfile,
      });

      when('method is called', () => {
        const bcscUser = bcscResolver.resolve();

        then('properties should be filled out', () => {
          expect(bcscUser.identityProvider).toBe(
            mockAccessTokenParsed.identity_provider,
          );
          expect(bcscUser.userId).toBe(mockAccessTokenParsed.sub);
          expect(bcscUser.idpId).toBe(mockBrokerProfile.username);
          expect(bcscUser.firstName).toBe(mockBrokerProfile.firstName);
          expect(bcscUser.lastName).toBe(mockBrokerProfile.lastName);
        });
      });
    });
  });
});
