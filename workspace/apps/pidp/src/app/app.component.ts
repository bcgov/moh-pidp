import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Data, Event } from '@angular/router';

import { Observable, map, mergeMap } from 'rxjs';

import { RouteStateService } from '@core/services/route-state.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  public title = 'Provider Identity Portal';

  public constructor(
    private activatedRoute: ActivatedRoute,
    private titleService: Title,
    private routeStateService: RouteStateService
  ) {}

  public ngOnInit(): void {
    const onNavEnd = this.routeStateService.onNavigationEnd();

    this.setPageTitle(onNavEnd);
  }

  /**
   * @description
   * Set the HTML page <title> on route event.
   */
  private setPageTitle(routeEvent: Observable<Event>): void {
    routeEvent
      .pipe(
        // Swap what is being observed to the activated route
        map(() => this.activatedRoute),
        // Find the last activated route by traversing over the state tree, and
        // then return it to the stream
        map((route: ActivatedRoute) => {
          while (route.firstChild) {
            route = route.firstChild;
          }
          return route;
        }),
        mergeMap((route: ActivatedRoute) => route.data)
      )
      .subscribe((routeData: Data) =>
        this.titleService.setTitle(routeData.title)
      );
  }
}
