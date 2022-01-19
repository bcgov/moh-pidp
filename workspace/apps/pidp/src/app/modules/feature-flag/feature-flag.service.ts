import { Injectable } from '@angular/core';

import { AccessTokenService } from '@app/features/auth/services/access-token.service';

// TODO replace with state management using Elf and use store query
@Injectable({
  providedIn: 'root',
})
export class FeatureFlagService {
  private readonly featureFlagKey: string;

  public constructor(private accessTokenService: AccessTokenService) {
    this.featureFlagKey = 'feature_';
  }

  public hasFlags(flags: string | string[]): boolean {
    flags = Array.isArray(flags) ? flags : [flags];

    const userFeatureFlags = this.accessTokenService
      .roles()
      .filter((role) => role.startsWith(this.featureFlagKey));

    return userFeatureFlags.some((flag) => flags.includes(flag));
  }
}
