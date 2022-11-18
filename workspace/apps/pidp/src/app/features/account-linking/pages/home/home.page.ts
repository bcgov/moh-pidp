import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

import { catchError } from 'rxjs';

import * as cryptojs from 'crypto-js';
import { KeycloakOptions, KeycloakService } from 'keycloak-angular';
import * as Keycloak from 'keycloak-js';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import { AuthService } from '@app/features/auth/services/auth.service';
import { KeycloakInitService } from '@app/modules/keycloak/keycloak-init.service';

@Component({
  selector: 'app-account-linking-home-page',
  templateUrl: './home.page.html',
  styleUrls: ['./home.page.scss'],
})
export class AccountLinkingHomePage {
  public messages: string[] = [];
  public constructor(
    private snackBar: MatSnackBar,
    @Inject(APP_CONFIG) private config: AppConfig,
    private initService: KeycloakInitService,
    private keycloakService: KeycloakService,
    private authService: AuthService,
    private http: HttpClient
  ) {}
  // public async load(): Promise<void> {
  //   const authenticated = await this.keycloakService.init(
  //     this.getKeycloakOptions()
  //   );

  //   this.keycloakService.getKeycloakInstance().onTokenExpired = (): void => {
  //     this.keycloakService
  //       .updateToken()
  //       .catch(() => this.router.navigateByUrl(AuthRoutes.MODULE_PATH));
  //   };

  //   if (authenticated) {
  //     // Force refresh to begin expiry timer
  //     await this.keycloakService.updateToken(-1);
  //   }
  // }

  // private getKeycloakOptions(): KeycloakOptions {
  //   return this.config.keycloakConfig;
  // }

  private clientId = 'PIDP-SERVICE'; // 'account'; // 'PIDP-WEBAPP';
  private realm = 'moh_applications';
  private url = 'https://common-logon-dev.hlth.gov.bc.ca/auth';

  public onLinkClick(): void {
    this.writeMessage('Starting to do something');

    // this.initService.load().then(() => {
    this.keycloakService
      .getToken()
      .then((token) => {
        this.writeMessage(`Got token: ${token}`);
      })
      .catch((error) => {
        this.writeMessage(`token error: ${error}`);
        console.log(error);
      });
    // });

    this.writeMessage('Finished doing something');
    this.snackBar.open('Just kidding!', 'OK');
  }

  public onLink2Click(): void {
    console.log(this.config.keycloakConfig);
    this.authService.getToken().subscribe((token) => {
      console.log(token);

      // You can extract the "session_state" from the JWT token at https://jwt.io/
      const sessionState = '09392d90-9e4e-4c0d-aa29-fce20fb658fe';
      const provider = 'bcsc_mspdirect';
      const nonce = '5c55cc82-c884-4e3e-9237-f03e2288f9b2';

      const payloadString = nonce + sessionState + this.clientId + provider;
      console.log(payloadString);
      const digest = cryptojs.SHA256(payloadString);
      console.log(digest);
      const base64Digest = cryptojs.enc.Base64.stringify(digest);
      console.log(base64Digest);

      const redirectUri = 'http://localhost:4200';
      const query = `?nonce=${nonce}&hash=${base64Digest}&client_id=${
        this.clientId
      }&redirect_uri=${encodeURIComponent(redirectUri)}`;
      const url = `${this.url}/realms/${this.realm}/broker/${provider}/link${query}`;

      console.log(url);

      const corsOrigin = 'https://dev.healthprovideridentityportal.gov.bc.ca/';
      //'http://localhost:4200'
      const headers = new HttpHeaders({
        // 'Content-Type': 'application/json; charset=utf-8}',
        'Access-Control-Allow-Origin': 'http://localhost:4200',
      });
      // const headers = new HttpHeaders().set('Access-Control-Allow-Origin', '*');
      this.http
        .get(url, { headers: headers })
        .pipe(
          catchError((err) => {
            console.log('get error', err);
            return ['error'];
          })
        )
        .subscribe((result) => {
          console.log('get result', result);
        });
    });
  }
  private writeMessage(s: string): void {
    this.messages.push(s);
  }

  // KeycloakSecurityContext session = (KeycloakSecurityContext) httpServletRequest.getAttribute(KeycloakSecurityContext.class.getName());
  //  AccessToken token = session.getToken();
  //  String clientId = token.getIssuedFor();
  //  String nonce = UUID.randomUUID().toString();
  //  MessageDigest md = null;
  //  try {
  //     md = MessageDigest.getInstance("SHA-256");
  //  } catch (NoSuchAlgorithmException e) {
  //     throw new RuntimeException(e);
  //  }
  //  String input = nonce + token.getSessionState() + clientId + provider;
  //  byte[] check = md.digest(input.getBytes(StandardCharsets.UTF_8));
  //  String hash = Base64Url.encode(check);
  //  request.getSession().setAttribute("hash", hash);
  //  String redirectUri = ...;
  //  String accountLinkUrl = KeycloakUriBuilder.fromUri(authServerRootUrl)
  //                   .path("/auth/realms/{realm}/broker/{provider}/link")
  //                   .queryParam("nonce", nonce)
  //                   .queryParam("hash", hash)
  //                   .queryParam("client_id", clientId)
  //                   .queryParam("redirect_uri", redirectUri).build(realm, provider).toString();
}
