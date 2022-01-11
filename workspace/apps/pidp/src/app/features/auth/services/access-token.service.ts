import { Injectable } from '@angular/core';

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

  public async token(): Promise<string> {
    return await this.keycloakService.getToken();
  }

  public isTokenExpired(): boolean {
    return this.keycloakService.isTokenExpired();
  }

  // public async identityProvider(): Promise<IdentityProviderEnum | null> {
  //   return await this.decodeToken().then(
  //     (token: AccessTokenParsed | null) => token?.identity_provider
  //   );
  // }

  public async decodeToken(): Promise<AccessTokenParsed | null> {
    const token = await this.token();
    return token ? this.jwtHelper.decodeToken(token) : null;
  }

  public clearToken(): void {
    this.keycloakService.clearToken();
  }
}
