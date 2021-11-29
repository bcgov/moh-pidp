import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { ConfigMap } from './environments/config-map.model';
import { environment } from './environments/environment';

import { AppModule } from './app/app.module';
import { APP_DI_CONFIG, AppConfig, APP_CONFIG } from './app/app-config.module';

// The deployment pipeline provides the config map based on environment
// without requiring a build by placing config-map.json within the public
// assets folder, otherwise it will not exist in local development which
// relies on the cascade of environment files.
fetch('/assets/config-map.json')
  .then((response) => response.json())
  .then((configMap: ConfigMap) => {
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

    platformBrowserDynamic([{ provide: APP_CONFIG, useValue: appConfig }])
      .bootstrapModule(AppModule)
      .catch((err) => console.error(err));
  });
