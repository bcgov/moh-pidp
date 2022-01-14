import { Observable, of } from 'rxjs';

import { KeycloakLoginOptions } from 'keycloak-js';

import { IAuthService } from '@app/features/auth/services/auth.service';

export class MockAuthService implements IAuthService {
  private _loggedIn: boolean;

  public constructor() {
    this._loggedIn = false;
  }

  public set loggedIn(loggedIn: boolean) {
    this._loggedIn = loggedIn;
  }

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  public login(options?: KeycloakLoginOptions): Promise<void> {
    return Promise.resolve();
  }

  public isLoggedIn(): Observable<boolean> {
    return of(this._loggedIn);
  }

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  public logout(redirectUri: string): Promise<void> {
    return Promise.resolve();
  }
}
