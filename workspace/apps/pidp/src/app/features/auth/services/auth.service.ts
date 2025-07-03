import { Injectable, inject } from '@angular/core';

import { Observable, from, of } from 'rxjs';

import Keycloak, { KeycloakLoginOptions } from 'keycloak-js';

export interface IAuthService {
  login(options?: KeycloakLoginOptions): Observable<void>;
  isLoggedIn(): Observable<boolean | undefined>;
  logout(redirectUri: string): Observable<void>;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService implements IAuthService {
  private readonly keycloak = inject(Keycloak);

  public login(options?: KeycloakLoginOptions): Observable<void> {
    return from(this.keycloak.login(options));
  }

  public isLoggedIn(): Observable<boolean> {
    return of(this.keycloak?.authenticated ?? false);
  }

  public logout(redirectUri: string): Observable<void> {
    return from(this.keycloak.logout({ redirectUri }));
  }
}
