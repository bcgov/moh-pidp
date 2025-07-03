import { NgIf } from '@angular/common';
import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';

import { InjectViewportCssClassDirective } from '../../directives/viewport-css.directive';
import { PidpViewport, ViewportService } from '../../services';
import { AnchorDirective } from '../anchor/anchor.directive';

@Component({
    selector: 'ui-layout-header-footer',
    templateUrl: './layout-header-footer.component.html',
    styleUrls: ['./layout-header-footer.component.scss'],
    imports: [AnchorDirective, InjectViewportCssClassDirective, NgIf]
})
export class LayoutHeaderFooterComponent {
  @Input() public emailSupport!: string;
  public isMobile = true;

  public constructor(
    viewportService: ViewportService,
    private readonly router: Router,
  ) {
    viewportService.viewportBroadcast$.subscribe((viewport) =>
      this.onViewportChange(viewport),
    );
  }

  public navigateToRoot(): void {
    this.router.navigateByUrl('/');
  }

  private onViewportChange(viewport: PidpViewport): void {
    this.isMobile = viewport === PidpViewport.xsmall;
  }
}
