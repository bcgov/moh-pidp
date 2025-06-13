import { Component } from '@angular/core';

import { RootRouteContainerComponent } from '../../shared/root-route-container/root-route-container.component';

@Component({
    selector: 'ui-maintenance',
    template: `
    <ui-root-route-container>
      <div class="row justify-content-center">
        <div class="col-sm-12 col-md-10 col-lg-8 text-center">
          <h1 class="mb-5">
            503
            <span class="d-block d-lg-inline">
              We're undergoing a bit of scheduled maintenance.
            </span>
          </h1>

          <p class="lead">
            Sorry for the inconvenience. We'll be back up and running as fast as
            possible.
          </p>
        </div>
      </div>
    </ui-root-route-container>
  `,
    styleUrls: ['../../shared/root-route-page-styles.scss'],
    imports: [RootRouteContainerComponent]
})
export class MaintenanceComponent {}
