import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { RouteUtils } from '@bcgov/shared/utils';

@Component({
  selector: 'app-page-not-found',
  template: `
    <app-root-route-container>
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
    </app-root-route-container>
  `,
  styleUrls: ['../../shared/root-route-page-styles.scss'],
})
export class PageNotFoundComponent {
  public constructor(private route: ActivatedRoute, private router: Router) {}

  public routeToRoot(): void {
    this.router.navigateByUrl(RouteUtils.currentModulePath(this.route));
  }
}
