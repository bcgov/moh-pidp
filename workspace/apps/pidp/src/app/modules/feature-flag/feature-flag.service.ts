import { Injectable } from '@angular/core';

import { AccessTokenService } from '@app/features/auth/services/access-token.service';

// TODO replace with state management using Elf
@Injectable({
  providedIn: 'root',
})
export class FeatureFlagService {
  private readonly featureFlagKey: string;

  public constructor(
    // TODO create authenticated user service for DI
    private accessTokenService: AccessTokenService
  ) {
    this.featureFlagKey = 'feature_';
  }

  // TODO enum provides flags, user has flags, server should indicate if flags are active?
  public hasFlag(flags: string | string[]): boolean {
    flags = Array.isArray(flags) ? flags : [flags];

    // TODO create helper method in authenticated user service for DI so not using low-level service
    const userFeatureFlags = this.accessTokenService
      .roles()
      .filter((role) => role.startsWith(this.featureFlagKey));

    return Array.isArray(flags)
      ? userFeatureFlags.some((flag) => flags.includes(flag))
      : userFeatureFlags.includes(flags);
  }
}
