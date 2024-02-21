import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { ActivatedRoute, Router } from '@angular/router';

import { RouteUtils } from '@bcgov/shared/utils';

import { RootRouteContainerComponent } from '../../shared/root-route-container/root-route-container.component';

@Component({
  selector: 'ui-access-denied',
  template: `
    <ui-root-route-container>
      <div class="row justify-content-center">
        <div class="col-sm-12 col-md-10 col-lg-8 text-center">
          <h1 class="mb-5">
            403
            <span class="d-block d-lg-inline">
              You don't appear to have the proper authorization.
            </span>
          </h1>

          <button mat-flat-button (click)="routeToRoot()">
            Let us help you find your way to your destination
          </button>
        </div>
      </div>
    </ui-root-route-container>
  `,
  styleUrls: ['../../shared/root-route-page-styles.scss'],
  standalone: true,
  imports: [RootRouteContainerComponent, MatButtonModule],
})
export class AccessDeniedComponent {
  public constructor(
    private route: ActivatedRoute,
    private router: Router,
  ) {}

  public routeToRoot(): void {
    this.router.navigateByUrl(RouteUtils.currentModulePath(this.route));
  }
}
