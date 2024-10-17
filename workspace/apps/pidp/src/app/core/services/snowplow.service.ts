import { Injectable } from '@angular/core';

import {
  ISnowplowWindow,
  WindowRefService,
} from '@core/services/window-ref.service';

@Injectable({
  providedIn: 'root',
})
export class SnowplowService {
  private _window: ISnowplowWindow;

  public constructor(window: WindowRefService) {
    this._window = window.nativeWindow;
    if (this._window.snowplow) {
      const collector = 'spm.apps.gov.bc.ca';
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

  /** Add Snowplow click listeners to all links which do not already have them */
  public refreshLinkClickTracking(): void {
    if (this._window.snowplow) {
      this._window.snowplow('refreshLinkClickTracking');
    }
  }
}
