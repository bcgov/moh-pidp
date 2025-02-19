import { Injectable } from '@angular/core';

import { Observable, from, of } from 'rxjs';

import { KeycloakService } from 'keycloak-angular';
import { KeycloakLoginOptions } from 'keycloak-js';

export interface IAuthService {
  login(options?: KeycloakLoginOptions): Observable<void>;
  isLoggedIn(): Observable<boolean>;
  logout(redirectUri: string): Observable<void>;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService implements IAuthService {
  public constructor(private readonly keycloakService: KeycloakService) {}

  public login(options?: KeycloakLoginOptions): Observable<void> {
    return from(this.keycloakService.login(options));
  }

  public isLoggedIn(): Observable<boolean> {
    return of(this.keycloakService.isLoggedIn());
  }

  public logout(redirectUri: string): Observable<void> {
    return from(this.keycloakService.logout(redirectUri));
  }
}
