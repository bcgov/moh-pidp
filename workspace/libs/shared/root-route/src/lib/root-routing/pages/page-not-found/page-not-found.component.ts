import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { ActivatedRoute, Router } from '@angular/router';

import { RouteUtils } from '@bcgov/shared/utils';

import { RootRouteContainerComponent } from '../../shared/root-route-container/root-route-container.component';

@Component({
    selector: 'ui-page-not-found',
    template: `
    <ui-root-route-container>
      <div class="row justify-content-center">
        <div class="col-sm-12 col-md-10 col-lg-8 text-center">
          <h1 class="mb-5">
            404
            <span class="d-block d-lg-inline">We couldn't find this page</span>
          </h1>

          <button mat-flat-button (click)="routeToRoot()">
            Let us help you find your way to your destination
          </button>
        </div>
      </div>
    </ui-root-route-container>
  `,
    styleUrls: ['../../shared/root-route-page-styles.scss'],
    imports: [RootRouteContainerComponent, MatButtonModule]
})
export class PageNotFoundComponent {
  public constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
  ) {}

  public routeToRoot(): void {
    this.router.navigateByUrl(RouteUtils.currentModulePath(this.route));
  }
}
