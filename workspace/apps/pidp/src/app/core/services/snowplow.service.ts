import { Inject, Injectable } from '@angular/core';

import { APP_CONFIG, AppConfig } from '@app/app.config';

import {
  ISnowplowWindow,
  WindowRefService,
} from '@core/services/window-ref.service';

import { EnvironmentName } from '../../../environments/environment.model';

@Injectable({
  providedIn: 'root',
})
export class SnowplowService {
  private _window: ISnowplowWindow;

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    window: WindowRefService,
  ) {
    this._window = window.nativeWindow;
    if (this._window.snowplow) {
      let collector: string;
      if (this.config.environmentName === EnvironmentName.PRODUCTION) {
        collector = 'spt.apps.gov.bc.ca';
      } else {
        collector = 'spm.apps.gov.bc.ca';
      }
      this._window.snowplow('newTracker', 'rt', collector, {
        appId: 'Snowplow_standalone',
        cookieLifetime: 86400 * 548,
        platform: 'web',
        post: true,
        forceSecureTracker: true,
        contexts: {
          webPage: true,
          performanceTiming: true,
        },
      });
      this._window.snowplow('enableActivityTracking', 30, 30); // Ping every 30 seconds after 30 seconds
      this._window.snowplow('enableLinkClickTracking');
    }
  }

  public trackPageView(): void {
    if (this._window.snowplow) {
      this._window.snowplow('trackPageView');
    }
  }

  // Add Snowplow click listeners to all links which do not already have them
  public refreshLinkClickTracking(): void {
    if (this._window.snowplow) {
      this._window.snowplow('refreshLinkClickTracking');
    }
  }

  // Add Snowplow click listeners to a custom click event
  public trackLinkClick(url: string): void {
    if (this._window.snowplow) {
      this._window.snowplow('trackLinkClick', url);
    }
  }
}
