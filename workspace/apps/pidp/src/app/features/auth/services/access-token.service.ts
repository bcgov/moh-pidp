import { Injectable } from '@angular/core';

import { Observable, from, map } from 'rxjs';

import { JwtHelperService } from '@auth0/angular-jwt';
import { KeycloakService } from 'keycloak-angular';

import { AccessTokenParsed } from '../models/access-token-parsed.model';
import { BrokerProfile } from '../models/broker-profile.model';

export interface IAccessTokenService {
  token(): Observable<string>;
  isTokenExpired(): boolean;
  decodeToken(): Observable<AccessTokenParsed | null>;
  roles(): string[];
  clearToken(): void;
}

@Injectable({
  providedIn: 'root',
})
export class AccessTokenService implements IAccessTokenService {
  private jwtHelper: JwtHelperService;

  public constructor(private keycloakService: KeycloakService) {
    this.jwtHelper = new JwtHelperService();
  }

  public token(): Observable<string> {
    return from(this.keycloakService.getToken());
  }

  public isTokenExpired(): boolean {
    return this.keycloakService.isTokenExpired();
  }

  public decodeToken(): Observable<AccessTokenParsed> {
    return this.token().pipe(
      map((token: string) => this.jwtHelper.decodeToken(token))
    );
  }

  public loadBrokerProfile(forceReload?: boolean): Observable<BrokerProfile> {
    return from(this.keycloakService.loadUserProfile(forceReload));
  }

  public roles(): string[] {
    return this.keycloakService.getUserRoles();
  }

  public clearToken(): void {
    this.keycloakService.clearToken();
  }
}
