import { Component, Input } from '@angular/core';

import { PidpViewport, ViewportService } from '../../services';
import { AnchorDirective } from '../anchor/anchor.directive';
import { NgIf } from '@angular/common';
import { InjectViewportCssClassDirective } from '../../directives/viewport-css.directive';

@Component({
    selector: 'ui-layout-header-footer',
    templateUrl: './layout-header-footer.component.html',
    styleUrls: ['./layout-header-footer.component.scss'],
    standalone: true,
    imports: [
        InjectViewportCssClassDirective,
        NgIf,
        AnchorDirective,
    ],
})
export class LayoutHeaderFooterComponent {
  @Input() public emailSupport!: string;
  public isMobile = true;

  public constructor(viewportService: ViewportService) {
    viewportService.viewportBroadcast$.subscribe((viewport) =>
      this.onViewportChange(viewport)
    );
  }
  private onViewportChange(viewport: PidpViewport): void {
    this.isMobile = viewport === PidpViewport.xsmall;
  }
}
