import { Observable, from } from 'rxjs';

import { KeycloakLoginOptions } from 'keycloak-js';

import { IdentityProviderEnum } from '@app/features/auth/enums/identity-provider.enum';
import { BcscUser } from '@app/features/auth/models/bcsc-user.model';
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

  public isLoggedIn(): Promise<boolean> {
    return new Promise((resolve, reject) => {
      this._loggedIn ? resolve(true) : reject(false);
    });
  }

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  public logout(redirectUri: string): Promise<void> {
    return Promise.resolve();
  }
}
