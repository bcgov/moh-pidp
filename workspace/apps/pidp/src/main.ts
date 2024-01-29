import { enableProdMode } from '@angular/core';
import { bootstrapApplication } from '@angular/platform-browser';

// import function to register Swiper custom elements
import { register } from 'swiper/element/bundle';

import { routes } from '@app/app-routing.routes';
import { provideCore } from '@app/core/core';

import { AppComponent } from './app/app.component';
import { APP_CONFIG, APP_DI_CONFIG, AppConfig } from './app/app.config';
import { environment } from './environments/environment';
import { EnvironmentConfig } from './environments/environment-config.model';

// register Swiper custom elements
register();

async function fetchConfig(): Promise<Response> {
  // Fetch environment.json.
  let response = await fetch('/assets/environment.json');

  // If environment.json is not found, look for environment.local.json.
  if (response.status === 404) {
    response = await fetch('/assets/environment.local.json');
  }
  return response;
}
function applySettingsToDefaultConfig(
  settingsToApply: EnvironmentConfig,
): AppConfig {
  let config = { ...APP_DI_CONFIG };

  if (settingsToApply) {
    // Extract immediate and child settings.
    // NOTE: Since the spread operator is a shallow copy, we must apply settings in child
    //       objects one by one.
    const { keycloakConfig, ...nonKeycloakConfig } = settingsToApply;

    // Apply root level settings to the default (= hard-coded) settings.
    config = { ...config, ...nonKeycloakConfig };

    // Apply child object settings to the default.

    // If keycloak.config is specified, apply it.
    if (keycloakConfig?.config) {
      // NOTE: keycloakConfig.config accepts "string | Keycloak.KeycloakConfig" so we must
      //       first cast to an object for the spread operator to work. Since we do not use
      //       the string config style for this setting, this cast should be ok.
      const oldKeycloakConfig = config.keycloakConfig
        .config as Keycloak.KeycloakConfig;
      const newKeycloakConfig =
        keycloakConfig.config as Keycloak.KeycloakConfig;
      config.keycloakConfig.config = {
        ...oldKeycloakConfig,
        ...newKeycloakConfig,
      };
    }
  }

  return config;
}
// The deployment pipeline provides the production environment configuration
// without requiring a rebuild by placing `environment.json` within the public
// assets folder, otherwise local development in/outside of a container relies
// on the local environment files.
fetchConfig()
  .then((response) => {
    return response.json();
  })
  .then((configMap: EnvironmentConfig) => {
    return applySettingsToDefaultConfig(configMap);
  })
  .catch((err) => {
    console.log(err);
    return APP_DI_CONFIG;
  })
  .then((appConfig: AppConfig) => {
    if (environment.production) {
      enableProdMode();
    }
    // For convenience, log the config being used when in development.
    if (appConfig.environmentName === 'dev') {
      console.log('pidp.config', appConfig);
    }
    bootstrapApplication(AppComponent, {
      providers: [
        {
          provide: APP_CONFIG,
          useValue: appConfig,
        },
        provideCore({ routes: routes }),
      ],
    }).catch((err) => console.error(err));
  });
