import { Component, Input } from '@angular/core';

import { PidpViewport, ViewportService } from '../../services';

@Component({
  selector: 'ui-layout-header-footer',
  templateUrl: './layout-header-footer.component.html',
  styleUrls: ['./layout-header-footer.component.scss'],
})
export class LayoutHeaderFooterComponent {
  @Input() public emailSupport!: string;
  public isMobile = true;

  public constructor(viewportService: ViewportService) {
    viewportService.viewportBroadcast$.subscribe((viewport) =>
      this.onViewportChange(viewport),
    );
  }
  private onViewportChange(viewport: PidpViewport): void {
    this.isMobile = viewport === PidpViewport.xsmall;
  }
}
