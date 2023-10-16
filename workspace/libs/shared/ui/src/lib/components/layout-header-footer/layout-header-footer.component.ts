import { Component, Inject } from '@angular/core';

import { APP_CONFIG, AppConfig } from '@app/app.config';

import { PidpViewport, ViewportService } from '../../services';

@Component({
  selector: 'ui-layout-header-footer',
  templateUrl: './layout-header-footer.component.html',
  styleUrls: ['./layout-header-footer.component.scss'],
})
export class LayoutHeaderFooterComponent {
  public isMobile = true;
  public providerIdentitySupport: string;

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    viewportService: ViewportService
  ) {
    viewportService.viewportBroadcast$.subscribe((viewport) =>
      this.onViewportChange(viewport)
    );
    this.providerIdentitySupport = this.config.emails.providerIdentitySupport;
  }
  private onViewportChange(viewport: PidpViewport): void {
    this.isMobile = viewport === PidpViewport.xsmall;
  }
}
