import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Data, Event, Scroll } from '@angular/router';

import { Observable, delay, filter, map, mergeMap } from 'rxjs';

import { contentContainerSelector } from '@bcgov/shared/ui';

import { RouteStateService } from '@core/services/route-state.service';
import { UtilsService } from '@core/services/utils.service';

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
    private routeStateService: RouteStateService,
    private utilsService: UtilsService
  ) {}

  public ngOnInit(): void {
    const onNavEnd = this.routeStateService.onNavigationEnd();

    this.setPageTitle(onNavEnd);
    // TODO don't scroll top when anchor scroll is about to occur
    this.scrollTop(onNavEnd);
    this.scrollToAnchor(this.routeStateService.onScrollEvent());
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

  /**
   * @description
   * Scroll to the top of the content container on NavigationEnd.
   */
  private scrollTop(routeEvent: Observable<Event>): void {
    routeEvent.subscribe(() =>
      this.utilsService.scrollTop(contentContainerSelector)
    );
  }

  /**
   * @description
   * Scroll to an anchor in the content container on Scroll event.
   */
  private scrollToAnchor(scroll: Observable<Scroll>): void {
    scroll
      .pipe(
        filter((event: Scroll) => !!event?.anchor),
        delay(500), // Provide settling time before triggering anchor scroll
        map((event: Scroll) => event.anchor)
      )
      .subscribe((routeFragment: string | null) =>
        this.utilsService.scrollToAnchor(routeFragment)
      );
  }
}
