import { Directive, HostBinding } from '@angular/core';

import { PidpViewport, ViewportService } from '../services';

/**
 * Inserts css classes that indicate the current viewport as well as a css class
 * that can be used to indicate css for all viewports.
 */
@Directive({
  selector: '[uiPidpInjectViewportCss]',
  standalone: true,
})
export class InjectViewportCssClassDirective {
  @HostBinding('class.viewport-xsmall') public viewportXsClass = false;
  @HostBinding('class.viewport-small') public viewportSClass = false;
  @HostBinding('class.viewport-medium') public viewportMClass = false;
  @HostBinding('class.viewport-large') public viewportLClass = false;
  @HostBinding('class.viewport-all') public viewportAllClass = true;

  public constructor(viewportService: ViewportService) {
    viewportService.viewportBroadcast$.subscribe((viewport) =>
      this.onViewportChange(viewport),
    );
  }

  private onViewportChange(viewport: PidpViewport): void {
    switch (viewport) {
      case PidpViewport.xsmall:
        this.viewportXsClass = true;
        this.viewportSClass = false;
        this.viewportMClass = false;
        this.viewportLClass = false;
        break;
      case PidpViewport.small:
        this.viewportXsClass = false;
        this.viewportSClass = true;
        this.viewportMClass = false;
        this.viewportLClass = false;
        break;
      case PidpViewport.medium:
        this.viewportXsClass = false;
        this.viewportSClass = false;
        this.viewportMClass = true;
        this.viewportLClass = false;
        break;
      case PidpViewport.large:
        this.viewportXsClass = false;
        this.viewportSClass = false;
        this.viewportMClass = false;
        this.viewportLClass = true;
        break;
      default:
        throw 'not implemented: ' + viewport;
    }
  }
}
