import { Directive, HostBinding } from '@angular/core';

import { PidpViewport, ViewportService } from '../services';

/**
 * Inserts css classes that indicate the current viewport as well as a css class
 * that can be used to indicate css for all viewports.
 */
@Directive({
  selector: '[uiPidpInjectViewportCss]',
})
export class InjectViewportCssClassDirective {
  @HostBinding('class.viewport-mobile') public viewportMobileClass = false;
  @HostBinding('class.viewport-desktop') public viewportDesktopClass = false;
  @HostBinding('class.viewport-all') public viewportAllClass = true;

  public constructor(viewportService: ViewportService) {
    console.log('inject');
    viewportService.viewportBroadcast$.subscribe((viewport) =>
      this.onViewportChange(viewport)
    );
  }

  private onViewportChange(viewport: PidpViewport): void {
    switch (viewport) {
      case PidpViewport.handset:
        this.viewportMobileClass = true;
        this.viewportDesktopClass = false;
        break;
      case PidpViewport.web:
        this.viewportMobileClass = false;
        this.viewportDesktopClass = true;
        break;
      default:
        throw 'not implemented: ' + viewport;
    }
  }
}
