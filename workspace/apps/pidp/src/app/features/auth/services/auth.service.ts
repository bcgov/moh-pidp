import { Injectable } from '@angular/core';

import { Observable, from } from 'rxjs';

import { KeycloakService } from 'keycloak-angular';
import { KeycloakLoginOptions } from 'keycloak-js';

export interface IAuthService {
  login(options?: KeycloakLoginOptions): Promise<void>;
  isLoggedIn(): Observable<boolean>;
  logout(redirectUri: string): Promise<void>;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService implements IAuthService {
  public constructor(private keycloakService: KeycloakService) {}

  public login(options?: KeycloakLoginOptions): Promise<void> {
    return this.keycloakService.login(options);
  }

  public isLoggedIn(): Observable<boolean> {
    return from(this.keycloakService.isLoggedIn());
  }

  public logout(redirectUri: string): Promise<void> {
    return this.keycloakService.logout(redirectUri);
  }
}
