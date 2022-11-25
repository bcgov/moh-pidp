import { Component, Inject } from '@angular/core';

import * as cryptojs from 'crypto-js';
import { KeycloakService } from 'keycloak-angular';
import { KeycloakConfig } from 'keycloak-js';

import { APP_CONFIG, AppConfig } from '@app/app.config';

@Component({
  selector: 'app-account-linking-home-page',
  templateUrl: './home.page.html',
  styleUrls: ['./home.page.scss'],
})
export class AccountLinkingHomePage {
  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private keycloakService: KeycloakService
  ) {}

  public onLinkClick(): void {
    // The code for the relevant endpoint on the server side can be found at:
    // https://github.com/keycloak/keycloak/blob/main/services/src/main/java/org/keycloak/services/resources/IdentityBrokerService.java#L207

    const nonce = '6dd7c817-7fd0-453c-a78a-035472985596'; // should be a random UUID
    const sessionState =
      this.keycloakService.getKeycloakInstance().tokenParsed.session_state;
    const clientId = 'PIDP-WEBAPP'; // the client the user has a session with: 'PIDP-WEBAPP' or 'account', see note at end
    const provider = 'bcsc'; // This is the IDP you are linking to, *not* the IDP you are currently logged into

    const payloadString = nonce + sessionState + clientId + provider;
    const digest = cryptojs.SHA256(payloadString);
    const hash = digest.toString(cryptojs.enc.Base64url);

    const redirectUri = encodeURIComponent(this.config.applicationUrl);
    const query = `?nonce=${nonce}&hash=${hash}&client_id=${clientId}&redirect_uri=${redirectUri}`;
    const keycloakUrl = (this.config.keycloakConfig.config as KeycloakConfig)
      .url;
    const url = `${keycloakUrl}/realms/moh_applications/broker/${provider}/link${query}`;

    window.location.replace(url);
  }
}

// Note on what clientId to use:
// Account linking requires either using the "account" client, or for the user to have the "manage account" or "manage account links" role in whatever client they are logged into.
// Users don't currently have that role in the PIDP-WEBAPP client, so using that clientId will result in a "not allowed" response (a 302 redirect to <redirect URI>/?link_error=not_allowed).
// Using "account" as the clientId causes a different error: a 400 Bad Request due to our application not being a valid redirect URI of the "account" client.

// Technically, the token has both "PIDP-WEBAPP" and "account" as audiances (the "aud" claim), but may in practice only have an active session with "PIDP-WEBAPP" and not "account".
// In that case, updating the valid redirect URIs of the "account" client and using "account" as the clientId will result in a 400 Bad Request as before but will be recorded in the error events as "INVALID_TOKEN" instead of "INVALID_REDIRECT_URI".
