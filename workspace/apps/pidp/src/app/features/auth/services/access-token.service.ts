import { Injectable, inject } from '@angular/core';

import { Observable, from, map, of } from 'rxjs';

import { JwtHelperService } from '@auth0/angular-jwt';
import Keycloak, { KeycloakTokenParsed } from 'keycloak-js';

import { AccessTokenParsed } from '../models/access-token-parsed.model';
import { BrokerProfile } from '../models/broker-profile.model';

export interface IAccessTokenService {
  token(): Observable<string>;
  isTokenExpired(): boolean;
  decodeToken(): Observable<KeycloakTokenParsed | null>;
  roles(): string[];
  clearToken(): void;
}

@Injectable({
  providedIn: 'root',
})
export class AccessTokenService implements IAccessTokenService {
  private readonly jwtHelper: JwtHelperService;
  private readonly keycloak = inject(Keycloak);

  public constructor() {
    this.jwtHelper = new JwtHelperService();
  }

  public token(): Observable<string> {
    if (!this.keycloak.token) {
      throw new Error('Keycloak token is not available');
    }
    return of(this.keycloak.token);
  }

  public isTokenExpired(): boolean {
    return this.keycloak.isTokenExpired();
  }

  public decodeToken(): Observable<AccessTokenParsed> {
    return this.token().pipe(
      map((token: string) => {
        const accessToken = this.jwtHelper.decodeToken(token);
        if (accessToken == null) {
          throw new Error('Token could not be decoded');
        }
        return accessToken;
      }),
    );
  }

  public loadBrokerProfile(): Observable<BrokerProfile> {
    return from(this.keycloak.loadUserProfile()) as Observable<BrokerProfile>;
  }

  public roles(): string[] {
    return this.keycloak.realmAccess?.roles ?? [];
  }

  public clearToken(): void {
    this.keycloak.clearToken();
  }
}
