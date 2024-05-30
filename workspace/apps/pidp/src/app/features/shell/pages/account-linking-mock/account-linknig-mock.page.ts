import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { PortalDashboardComponent } from '../../components/portal-dashboard/portal-dashboard.component';
import { PidpViewport, ViewportService } from '@bcgov/shared/ui';
import { NgIf } from '@angular/common';


@Component({
  selector: 'account-linking-mock-page',
  templateUrl: './account-linking-mock.page.html',
  styleUrls: ['./account-linking-mock.page.scss'],
  standalone: true,
  imports: [
    NgIf,
    PortalDashboardComponent
],
})
export class AccountLinkingMockPage {

  public viewport = PidpViewport.xsmall;
  public isMobileView = false;
  public isTabletPortraitMode = false;
  public isTabletLandscapeMode = false;
  public isWebView = false;

  public constructor(
    private router: Router,
    private viewportService: ViewportService
  ) {
    this.viewportService.viewportBroadcast$.subscribe((viewport) =>
        this.onViewportChange(viewport),
      ); 
  }

  private onViewportChange(viewport: PidpViewport): void {
    this.viewport = viewport;
    this.isMobileView = false;
    this.isTabletPortraitMode = false;
    this.isTabletLandscapeMode = false;
    this.isWebView = false;

    switch (this.viewport) {
      case PidpViewport.xsmall:
        this.isMobileView = true;
        break;
      case PidpViewport.small:
        this.isTabletPortraitMode = true;
        break;
      case PidpViewport.medium:
        this.isTabletLandscapeMode = true;
        break;
      case PidpViewport.large:
        this.isWebView = true;
        break;
      default:
        throw 'not implemented: ' + this.viewport;
    }
  }
 
}
