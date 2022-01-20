import { Observable, OperatorFunction, combineLatest } from 'rxjs';

import { AccessTokenParsed } from '../models/access-token-parsed.model';
import { BrokerProfile } from '../models/broker-profile.model';
import { AccessTokenService } from '../services/access-token.service';

export interface UserIdentity {
  accessTokenParsed: AccessTokenParsed | null;
  brokerProfile: BrokerProfile;
}

/**
 * @description
 * Abstract service for creating a type of authorized
 * user from access token claims (user identity).
 */
export abstract class AbstractAuthorizedUserService<T> {
  public constructor(private accessTokenService: AccessTokenService) {}

  /**
   * @description
   * Get the authorized user mapped from the access token.
   */
  public get user$(): Observable<T | null> {
    return this.getUserIdentity().pipe(this.fromUserIdentity());
  }

  /**
   * @description
   * Get the user roles from the access token.
   */
  public get roles(): string[] {
    return this.accessTokenService.roles();
  }

  /**
   * @description
   * Get the claims (user identity) from the access token.
   */
  protected getUserIdentity(): Observable<UserIdentity> {
    return combineLatest({
      accessTokenParsed: this.accessTokenService.decodeToken(),
      brokerProfile: this.accessTokenService.loadBrokerProfile(),
    });
  }

  /**
   * @description
   * Map the user identity to a generic model.
   */
  protected abstract fromUserIdentity(): OperatorFunction<
    UserIdentity,
    T | null
  >;
}
