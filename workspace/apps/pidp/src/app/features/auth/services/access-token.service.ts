import { Injectable } from '@angular/core';

import { Observable, from, map } from 'rxjs';

import { JwtHelperService } from '@auth0/angular-jwt';
import { KeycloakService } from 'keycloak-angular';

import { AccessTokenParsed } from '../models/access-token-parsed.model';

@Injectable({
  providedIn: 'root',
})
export class AccessTokenService {
  private jwtHelper: JwtHelperService;

  public constructor(private keycloakService: KeycloakService) {
    this.jwtHelper = new JwtHelperService();
  }

  public token$(): Observable<string> {
    return from(this.keycloakService.getToken());
  }

  public isTokenExpired(): boolean {
    return this.keycloakService.isTokenExpired();
  }

  public decodeToken$(): Observable<AccessTokenParsed | null> {
    return this.token$().pipe(
      map((token: string) => (token ? this.jwtHelper.decodeToken(token) : null))
    );
  }

  public roles(): string[] {
    return this.keycloakService.getUserRoles();
  }

  public clearToken(): void {
    this.keycloakService.clearToken();
  }
}
