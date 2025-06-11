import { Inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';

import { KeycloakService, ProvideKeycloakOptions } from 'keycloak-angular';

import { APP_CONFIG, AppConfig } from '@app/app.config';

import { AuthRoutes } from '../../features/auth/auth.routes';

@Injectable({
  providedIn: 'root',
})
export class KeycloakInitService {
  public constructor(
    @Inject(APP_CONFIG) private readonly config: AppConfig,
    private readonly router: Router,
    private readonly keycloakService: KeycloakService,
  ) {}

  public async load(): Promise<void> {
    const authenticated = await this.keycloakService.init(
      this.getKeycloakOptions(),
    );

    this.keycloakService.getKeycloakInstance().onTokenExpired = (): void => {
      this.keycloakService
        .updateToken()
        .catch(() => this.router.navigateByUrl(AuthRoutes.BASE_PATH));
    };

    if (authenticated) {
      // Force refresh to begin expiry timer
      await this.keycloakService.updateToken(-1);
    }
  }

  private getKeycloakOptions(): ProvideKeycloakOptions {
    return this.config.keycloakConfig;
  }
}
