import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { APP_DI_CONFIG, APP_CONFIG, AppConfig } from './app/app.config';
import { AppModule } from './app/app.module';
import { environment } from './environments/environment';
import { EnvironmentConfig } from './environments/environment-config.model';

// The deployment pipeline provides the production environment configuration
// without requiring a rebuild by placing `environment.json` within the public
// assets folder, otherwise local development in/outside of a container relies
// on the local environment files.
fetch('/assets/environment.json')
  .then((response) => response.json())
  .then((configMap: EnvironmentConfig) => {
    let appConfig = APP_DI_CONFIG;

    if (configMap) {
      const { keycloakConfig, ...root } = configMap;
      appConfig = { ...appConfig, ...root };
      appConfig.keycloakConfig.config = keycloakConfig.config;
    }

    return appConfig;
  })
  .catch(() => APP_DI_CONFIG)
  .then((appConfig: AppConfig) => {
    if (environment.production) {
      enableProdMode();
    }

    platformBrowserDynamic([
      {
        provide: APP_CONFIG,
        useValue: appConfig,
      },
    ])
      .bootstrapModule(AppModule)
      .catch((err) => console.error(err));
  });
