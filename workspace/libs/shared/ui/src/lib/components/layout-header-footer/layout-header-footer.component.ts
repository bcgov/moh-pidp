import { NgIf } from '@angular/common';
import { Component, Input } from '@angular/core';

import { InjectViewportCssClassDirective } from '../../directives/viewport-css.directive';
import { PidpViewport, ViewportService } from '../../services';
import { AnchorDirective } from '../anchor/anchor.directive';
import { NavigationService } from '@pidp/presentation';

@Component({
  selector: 'ui-layout-header-footer',
  templateUrl: './layout-header-footer.component.html',
  styleUrls: ['./layout-header-footer.component.scss'],
  standalone: true,
  imports: [AnchorDirective, InjectViewportCssClassDirective, NgIf],
})
export class LayoutHeaderFooterComponent {
  @Input() public emailSupport!: string;
  public isMobile = true;

  public constructor(viewportService: ViewportService,
    private navigationService: NavigationService) {
    viewportService.viewportBroadcast$.subscribe((viewport) =>
      this.onViewportChange(viewport),
    );
  }

  public navigateToRoot(): void {
    this.navigationService.navigateToRoot();
  }

  private onViewportChange(viewport: PidpViewport): void {
    this.isMobile = viewport === PidpViewport.xsmall;
  }
}
