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
import { BrokerProfile } from './broker-profile.model';
import { PhsaResolver, PhsaUser } from './phsa-user.model';

describe('PhsaUser', () => {
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
        identity_provider: IdentityProvider.PHSA,
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
        const phsaUser = new PhsaUser({
          accessTokenParsed: mockAccessTokenParsed,
          brokerProfile: mockBrokerProfile,
        });

        then('properties should be filled out', () => {
          expect(phsaUser.identityProvider).toBe(
            mockAccessTokenParsed.identity_provider,
          );
          expect(phsaUser.userId).toBe(mockAccessTokenParsed.sub);
          expect(phsaUser.idpId).toBe(mockBrokerProfile.username);
          expect(phsaUser.firstName).toBe(mockBrokerProfile.firstName);
          expect(phsaUser.lastName).toBe(mockBrokerProfile.lastName);
          expect(phsaUser.email).toBe(mockBrokerProfile.email);
        });
      });
    });
  });
});

describe('PhsaResolver', () => {
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
        identity_provider: IdentityProvider.PHSA,
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
      const phsaResolver = new PhsaResolver({
        accessTokenParsed: mockAccessTokenParsed,
        brokerProfile: mockBrokerProfile,
      });

      when('method is called', () => {
        const phsaUser = phsaResolver.resolve();

        then('properties should be filled out', () => {
          expect(phsaUser.identityProvider).toBe(
            mockAccessTokenParsed.identity_provider,
          );
          expect(phsaUser.userId).toBe(mockAccessTokenParsed.sub);
          expect(phsaUser.idpId).toBe(mockBrokerProfile.username);
          expect(phsaUser.firstName).toBe(mockBrokerProfile.firstName);
          expect(phsaUser.lastName).toBe(mockBrokerProfile.lastName);
        });
      });
    });
  });
});
